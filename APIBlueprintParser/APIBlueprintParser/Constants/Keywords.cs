//
// Keywords.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using System.Linq;
using System.Collections.Generic;

namespace APIBlueprintParser.Constants {
    
    public static class Keywords {
        public const string Request = "Request";
    }
}

namespace System {

	public static class StringExtension {

		public static IEnumerable<string> Words(this string source) {
			char[] wordSeparators = { ' ', '\t', '\n', '\r' ,'(',')','!', ',','[',']' };

			var trimed = source.Trim();

			var currentWord = "";

			for (int i = 0; i < source.Length; i++) {
				if(wordSeparators.Contains(source[i])) {
                    yield return currentWord.Trim();
					currentWord = "";
				} else {
					currentWord += source[i];
				}
			}
            yield return currentWord.Trim();
		}

	}

}
