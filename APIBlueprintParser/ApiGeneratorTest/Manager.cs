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

        public class DataSet {
            public string ProjectName { get; set; }
            public int Port { get; set; }
        }

        private static string PathToRoot = Constants.RootDirectory + Constants.ServersProjectDir + "/.store.json";
        private const int FirstPort = 11114;
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
            DataSet readedProject = readed.Find(dataSet => dataSet.ProjectName.Equals(projectName));

            if (readedProject != null)
            {
                return readedProject.Port;
            }

            var res = new DataSet();
            if (readed.Count != 0) {
                port = readed.Select(x => x.Port).Max() + 1;
            }
            res.Port = port;
            res.ProjectName = projectName;
            readed.Add(res);

            File.WriteAllText(PathToRoot, JsonConvert.SerializeObject(readed.ToArray()));

            return port;
        }
    }
}
