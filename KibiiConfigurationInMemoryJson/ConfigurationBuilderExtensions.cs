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

/* Author: Sarmad Wahab (bin) @ Kibii Inc. */

using System;
using System.Text;
using Kibii.Extensions.Configuration.InMemoryJson.FileInfoProviders;
using Kibii.Extensions.Configuration.InMemoryJson.FileProviders;
using Microsoft.Extensions.Configuration;

namespace Kibii.Extensions.Configuration.InMemoryJson
{
    public static class ConfigurationBuilderExtensions
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Adds the specified JSON into the IConfigurationBuilder as if adding a real JSON file.
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="inMemoryJson">The JSON data to use in the configuration.</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInMemoryJson(this IConfigurationBuilder configurationBuilder, string inMemoryJson)
        {
            string temporaryFileName = "in_memory_json_" + _random.Next(-1, int.MaxValue).ToString("D16") + ".json";

            InMemoryFileInfo fileInfo = new InMemoryFileInfo(temporaryFileName, Encoding.UTF8.GetBytes(inMemoryJson), DateTimeOffset.Now);
            InMemoryFileProvider @in = new InMemoryFileProvider(fileInfo);

            return configurationBuilder.AddJsonFile(@in, temporaryFileName, optional: false, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the specified JSON into the IConfigurationBuilder as if adding a real JSON file.
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <param name="inMemoryJson">A method that will return a JSON string when called.</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInMemoryJson(this IConfigurationBuilder configurationBuilder, Func<string> jsonConfigurationSource) =>
            configurationBuilder.AddInMemoryJson(jsonConfigurationSource());
    }
}
