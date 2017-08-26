//
// Extensions.cs
// 17.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//

using System.IO;

namespace APIBlueprintTests {

    public static class Extensions {

        public static StreamReader CreatFromString(string str) {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.WriteLine(str);
            writer.Flush();
            memoryStream.Position = 0;
            return new StreamReader(memoryStream);
        }
    }
}