﻿// -----------------------------------------------------------------------
//  <copyright file="IProjectFactory.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.IO;

namespace SqlG
{
    /// <summary>
    /// Creates an instance of a project, regardless of target framework
    /// </summary>
    internal interface IProjectFactory
    {
        /// <summary>
        /// Gets a new instance based on the provided file
        /// </summary>
        /// <param name="projectFile"></param>
        /// <returns></returns>
        Project GetProject(FileInfo projectFile);
    }
}