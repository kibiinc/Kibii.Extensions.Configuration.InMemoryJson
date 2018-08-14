#region License Information (MIT)

/*
    MIT License

    Copyright (c) 2018 Kibii Inc.

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.

*/

#endregion

/* Forked from https://github.com/aspnet/FileSystem/blob/master/test/FS.Composite.Tests/MockFileProvider.cs */
/* Author: natemcmaster @ Microsoft. Minor changes made by Kibii Inc. */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Kibii.Extensions.Configuration.InMemoryJson.FileProviders
{
    public class InMemoryFileProvider : IFileProvider
    {
        private IEnumerable<IFileInfo> _files;
        private Dictionary<string, IChangeToken> _changeTokens;

        public InMemoryFileProvider() { }
        public InMemoryFileProvider(params IFileInfo[] files)
        {
            _files = files;
        }

        public InMemoryFileProvider(params KeyValuePair<string, IChangeToken>[] changeTokens)
        {
            _changeTokens = changeTokens.ToDictionary(
                changeToken => changeToken.Key,
                changeToken => changeToken.Value,
                StringComparer.Ordinal);
        }

        public IDirectoryContents GetDirectoryContents(string subPath)
        {
            Mock<IDirectoryContents> contents = new Mock<IDirectoryContents>();
            contents.Setup(m => m.Exists).Returns(true);

            if (string.IsNullOrEmpty(subPath))
            {
                contents.Setup(m => m.GetEnumerator()).Returns(_files.GetEnumerator());
                return contents.Object;
            }

            IEnumerable<IFileInfo> filesInFolder = _files.Where(f => f.Name.StartsWith(subPath, StringComparison.Ordinal));
            if (filesInFolder.Any())
            {
                contents.Setup(m => m.GetEnumerator()).Returns(filesInFolder.GetEnumerator());
                return contents.Object;
            }

            return NotFoundDirectoryContents.Singleton;
        }

        public IFileInfo GetFileInfo(string subPath)
        {
            IFileInfo file = _files.FirstOrDefault(f => f.Name == subPath);
            return file ?? new NotFoundFileInfo(subPath);
        }

        public IChangeToken Watch(string filter)
        {
            if (_changeTokens != null && _changeTokens.ContainsKey(filter))
            {
                return _changeTokens[filter];
            }

            return NullChangeToken.Singleton;
        }
    }
}