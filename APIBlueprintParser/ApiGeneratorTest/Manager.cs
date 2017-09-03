//
// Manager.cs
// 31.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

namespace ApiGeneratorTest {
    
    public class Manager {

        public struct DataSet {
            public string ProjectName { get; set; }
            public int Port { get; set; }
        }

        private static string PathToRoot = Constants.RootDirectory + Constants.ServersProjectDir + "/.store.json";
        private const int FirstPort = 11111;
        /// <summary>
        /// Adds the new server.
        /// </summary>
        /// <returns>Free port</returns>
        public int AddNewServer(string projectName) {

            if (!File.Exists(PathToRoot)) {
                File.WriteAllText(PathToRoot,"[]");
            }

            var readed = JsonConvert.DeserializeObject<DataSet[]>(File.ReadAllText(PathToRoot)).ToList();

            var port = FirstPort;

            if (readed.Count != 0) {
                port = readed.Select(arg => arg.Port).Max() + 1;
            }

            var res = new DataSet();
            res.Port = port;
            res.ProjectName = projectName;
            readed.Add(res);

            File.WriteAllText(PathToRoot, JsonConvert.SerializeObject(readed.ToArray()));

            return port;
        }
    }
}
