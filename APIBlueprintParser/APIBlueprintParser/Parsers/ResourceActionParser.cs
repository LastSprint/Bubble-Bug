//
// GroupParser.cs
// 23.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using APIBlueprintParser.Models;

namespace APIBlueprintParser.Parsers {

	/// <summary>
	/// Realy format: ### <identifier> [<httpMethod>,<URITemplate>] ...
	/// Input foramt: <identifier> [<httpMethod>,<URITemplate>] ...
	/// </summary>
	public class ResourceActionParser: BaseParser {

		#region Nested

		public struct Tokens {
			public const char EndOfHeader = '+';
			public const char StartOfTypeSection = '[';
			public const char EndOfTypeSection = ']';
			public const char TypeSectionSeparator = ',';

			public const int HttpMethodTypeIndex = 0;
			public const int URITemplateTypeIndex = 1;
		}

  		#endregion

		public ResourceActionParser(Stream stream): base(stream) { }

		/// <summary>
		/// Parses the without nesten nodes.
		/// Implement for testable.
		/// </summary>
		/// <returns>ResourceActionNode without nested nodes</returns>
		public ResourceActionNode ParseWithoutNestenNodes() {

			// <identifier> [<httpmeth>, <URITempl>] .... +
			// read to '+'
			var streamReader = new StreamReader(base.stream);
			var sectionCharArr = new List<char>();

			while (!streamReader.EndOfStream && streamReader.Peek() != Tokens.EndOfHeader) {
				sectionCharArr.Add((char)streamReader.Read());
			}

			// <identifier> [<httpmeth>, <URITempl>] ....
			var stringView = new String(sectionCharArr.ToArray()).Trim();

			// check format

			if (!stringView.Contains(Tokens.StartOfTypeSection)) {
				throw new FormatException($"Header doesnt contains \'{Tokens.StartOfTypeSection}\'");
			}

			if (!stringView.Contains(Tokens.EndOfTypeSection)) {
				throw new FormatException($"Header doesnt contains \'{Tokens.StartOfTypeSection}\'");
			}

			// read <identifier>

			var identifier = "";

			for (int i = 0; i < stringView.Length && stringView[i] != Tokens.StartOfTypeSection; i++ ) {
				identifier += stringView[i];
			}

			identifier = identifier.Trim();

			// read [<httpMeth>, <URITempl>]

			var typeSection = "";

			var indexOfStartTypeSection = stringView.IndexOf(Tokens.StartOfTypeSection);

			for (int i = indexOfStartTypeSection + 1; i < stringView.Length && stringView[i] != Tokens.EndOfTypeSection; i++) {
				typeSection += stringView[i];
			}

			typeSection = typeSection.Trim();
			// validation

			if (!typeSection.Contains(Tokens.TypeSectionSeparator)) {
				throw new FormatException($"Type section doesnt contains \'{Tokens.TypeSectionSeparator}\'");
			}

			// split <HttpMeth>, <URITempl> by ','

			var splited = typeSection.Split(Tokens.TypeSectionSeparator);

			if (splited.Length != 2) {
				throw new FormatException("Type section doesnt contains HttpMethod and/or URITemplate or contains too much arguments");
			}

			var httpMethStringView = splited[Tokens.HttpMethodTypeIndex].Trim();
			var URITemplStringView = splited[Tokens.URITemplateTypeIndex].Trim();

			var httpMeth = Support.StringToHttpMethod(httpMethStringView);

			if (!httpMeth.HasValue) {
				throw new FormatException("Cant parse http method");
			}

			if (!HeaderKeywords.IsURI(URITemplStringView)) {
				throw new FormatException("Cant parse URI template");
			}

			return new ResourceActionNode(identifier, new UriTemplate.Core.UriTemplate(URITemplStringView), httpMeth.Value);
		}
	}
}
