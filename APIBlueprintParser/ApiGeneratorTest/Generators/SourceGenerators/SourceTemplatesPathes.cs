﻿//
// GlobalPathes.cs
// 27.08.2017
// Created by Kravchenkov Alexander
// sprintend@gmail.com
//
//
using System;
using ApiGeneratorTest;

namespace ApiGeneratorTest.Generators.SourceGenerators {
    public static class SourceTemplatesPathes {
        public static string PathToTemplates = Constants.RootDirectory + "/Templates/SourceTemplates";

        public static string PathToMethodTemplate = PathToTemplates + "/Method.template";
        public static string PathToControllerTemplate = PathToTemplates + "/Controller.template";
    }
}
