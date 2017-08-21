//
// BaseParser.cs
// 21.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System;
using System.IO;

namespace APIBlueprintParser.Parsers {
    
    public abstract class BaseParser {

        protected Stream stream;

        protected BaseParser(Stream stream) {
            this.stream = stream;
        }
    }
}
