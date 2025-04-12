//
// PdfMerger
//
// Copyright (c) 2023-2025 paoldev
//
// Licensed under the MIT license.
// SPDX-License-Identifier: MIT
//

namespace PdfMerger
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}