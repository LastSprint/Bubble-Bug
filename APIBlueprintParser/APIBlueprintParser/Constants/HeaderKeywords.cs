//
// Keywords.cs
// 20.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.Collections.Generic;
using System; 



public static class HeaderKeywords {
    
	// TODO: Refactor this file. Validation method need sends to Support.cs

    public const string Group = "Group";
    public const string DataStructures = "Data Structures";

    public static IReadOnlyList<string> HttpMethods = new List<string> { "GET", "POST", "PUT", "DELETE", "UPDATE" };

    /// <summary>
    /// Check if string is URI 
    /// </summary>
    /// <returns><c>true</c>, if string is URI, <c>false</c> otherwise.</returns>
    /// <param name="str">String.</param>
    public static bool IsURI(string str) {

        if(str[0] != '/') {
            return false;
        }

        try {
            var t = new UriTemplate.Core.UriTemplate(str);
        } catch {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Check if token is not a any HeaderKeyword.
    /// </summary>
    /// <returns><c>true</c>, if token is not a HeaderKeyword, <c>false</c> otherwise.</returns>
    /// <param name="token">Token.</param>
    public static bool IsNonKeyword(string token) {
        bool notGroup = String.Compare(token, Group, StringComparison.InvariantCulture) != 0;
        bool notDataStructures = String.Compare(token, DataStructures, StringComparison.InvariantCulture) != 0;

        bool notHttpMethod = true;

        foreach (var method in HttpMethods) {
            notHttpMethod &= String.Compare(token, method, StringComparison.InvariantCulture) != 0;
        }

        bool notURI = !IsURI(token);

        return notGroup && notDataStructures && notHttpMethod && notURI;
    }
}