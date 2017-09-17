//
// EquatableRequaest.cs
// 29.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using APIBlueprintParser.Models;
using Newtonsoft.Json.Linq;

namespace ApiGeneratorTest.ThirdParty
{

    public enum ParameterType
    {
        Optional = 0,
        Required = 1
    }

    public enum ValueType
    {
        Object = 0,
        Number = 1,
        String = 2,
        Bool = 3,
        Long = 4
    }

    public struct RequestParameter
    {
        public ParameterType Type { get; private set; }
        public ValueType ValueType { get; private set; }

        public object Value { get; private set; }
        public string Name { get; private set; }

        public RequestParameter(ParameterType paramType, ValueType valueType, object value, string name)
        {
            this.ValueType = valueType;
            this.Type = paramType;
            this.Value = value;
            this.Name = name;
        }

        public RequestParameter(object value, string name, ParameterType type) : this()
        {
            this.ValueType = this.GetType(value);
            this.Type = type;
            this.Value = value;
            this.Name = name;
        }

        public RequestParameter(AttributeNode node, object value) : this()
        {
            this.Name = node.Name;
            this.Type = this.ParameterTypeConverter(node.NeededType);
            this.ValueType = this.ValueTypeConverter(node.ValueType);
            this.Value = value;
        }

        private ValueType ValueTypeConverter(APIBlueprintParser.Models.ValueType type)
        {
            switch (type)
            {
                case APIBlueprintParser.Models.ValueType.Bool: return ValueType.Bool;
                case APIBlueprintParser.Models.ValueType.Number: return ValueType.Number;
                case APIBlueprintParser.Models.ValueType.Object: return ValueType.Object;
                case APIBlueprintParser.Models.ValueType.String: return ValueType.String;
                case APIBlueprintParser.Models.ValueType.Long: return ValueType.Long;
            }

            throw new ArgumentOutOfRangeException();
        }

        private ParameterType ParameterTypeConverter(NeededType type)
        {
            switch (type)
            {
                case NeededType.Optional: return ParameterType.Optional;
                case NeededType.Required: return ParameterType.Required;
            }

            throw new ArgumentOutOfRangeException();
        }

        private ValueType GetType(object obj)
        {
            if (obj is bool)
            {
                return ValueType.Bool;
            }

            if (obj is int)
            {
                return ValueType.Number;
            }
            if (obj is string)
            {
                return ValueType.String;
            }

            return ValueType.Object;
        }

        public override bool Equals(object obj)
        {

            if (obj == null)
            {
                return false;
            }

            if (!(obj is RequestParameter))
            {
                return false;
            }

            var casted = (RequestParameter)obj;

            if (this.Name != casted.Name)
            {
                return false;
            }

            return this.Value.Equals(casted.Value);
        }

        public override int GetHashCode()
        {
            return this.Name.ToCharArray().Cast<int>().Aggregate((x, y) => x ^ y);
        }
    }

    public class EquatableRequaest
    {

        // Response
        public object ResponseBody { get; }
        public int ResponseCode { get; }
        public IReadOnlyDictionary<string, string> ResponseHeaders { get; }

        // Request

        public IList<RequestParameter> Parameters { get; }
        public IReadOnlyDictionary<string, string> RequestHeaders { get; }
        public object RequestBody { get; }

        public EquatableRequaest(object responseBody, int responseCode, IReadOnlyDictionary<string, string> responseHeaders, IList<RequestParameter> parameters, IReadOnlyDictionary<string, string> requestHeaders, object body)
        {
            this.ResponseBody = responseBody;
            this.ResponseCode = responseCode;
            this.ResponseHeaders = responseHeaders;
            this.RequestHeaders = requestHeaders;
            this.Parameters = parameters;
            this.ResponseBody = body;
        }

        public EquatableRequaest(object requestBody, IList<RequestParameter> parameters)
        {
            this.RequestBody = requestBody;
            this.Parameters = parameters;
        }

        public EquatableRequaest(RequestPair pair, IReadOnlyList<AttributeNode> attributes)
        {
            this.ResponseBody = pair.Response.Body;
            if (pair.Response.Body != null)
            {
				try {
					this.ResponseBody = JObject.Parse(pair.Response.Body);
				} catch (Exception e) {
					this.ResponseBody = JArray.Parse(pair.Response.Body);
				}
            }
            this.ResponseCode = pair.Response.Code;
            this.ResponseHeaders = pair.Response.Headers;

            this.RequestHeaders = pair.Request.Headers;

			if (pair.Request.Body != null) {
				this.RequestBody = JObject.Parse(pair.Request.Body);
			}

            this.Parameters = new List<RequestParameter>();

            if (pair.Request.Parameters != null)
            {
                foreach (var smth in pair.Request.Parameters)
                {
                    var attr = attributes.First( x => x.Name.Equals(smth.Key));
                    this.Parameters.Add(new RequestParameter(attr, smth.Value));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var casted = obj as EquatableRequaest;

            if (casted == null)
            {
                return false;
            }

            for (int i = 0; i < this.Parameters.Count; i++)
            {

                if (!this.Parameters[i].Equals(casted.Parameters[i]))
                {
                    return false;
                }
            }

			if(!casted.RequestBody.Equals(this.RequestBody)) {
				return false;
			}

			var castedHeaders = casted.RequestHeaders;


			if (castedHeaders == null && this.RequestHeaders == null) {
				return true;
			}

			if (castedHeaders == null && this.RequestHeaders != null) {
				return false;
			}

			if (castedHeaders != null && this.RequestHeaders == null) {
				return false;
			}

			foreach (var header in castedHeaders) {

				if (!this.RequestHeaders.TryGetValue(header.Key, out string value)) {
					continue;
				}

				if (value != header.Value) {
					return false;
				}
			}

			return true;
        }

        public override int GetHashCode()
        {
            return this.Parameters.ToArray().GetHashCode();
        }
    }
}