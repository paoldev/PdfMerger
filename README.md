# PdfMerger

[![License: MIT](https://img.shields.io/badge/License-MIT-red.svg)](LICENSE.txt)
[![Build and Create Release](https://github.com/paoldev/PdfMerger/actions/workflows/dotnet_create_release.yml/badge.svg)](https://github.com/paoldev/PdfMerger/releases)

Merge two or more PDF files into one PDF file.
  
The resulting file is generated through rasterization, so its size is, in general, greater than the sum of the original files' sizes.
  
Additional features:  
* Load images in common formats
* Save one pdf or one image per page: the output file naming convention is `filename_pag_{number}.ext` (i.e. `singlepage_pag_1.pdf`, `singlepage_pag_2.pdf`, etc.)
* Save files as OpenXPS files (\*.oxps) or XPS files (\*.xps)
* Merge specific page ranges; pages can also be duplicated and merged in reverse order.  
For example, specifying `2-5, 7, 20-15, 1, 3` generates the output pages in the following order: `2,3,4,5,7,20,19,18,17,16,15,1,3`.
  
This application requires the [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0); it can be automatically downloaded and installed on the application's first run.  
  
# License

This project is licensed under the terms of the [MIT license](./LICENSE.txt).
