//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// Represents a query in a Uri.
    /// </summary>
    /// <remarks>
    /// This class can be used to parse a query string to access 
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class UriQuery : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly List<KeyValuePair<string, string>> entries = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQuery"/> class.
        /// </summary>
        public UriQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQuery"/> class with a query string.
        /// </summary>
        /// <param name="query">The query string.</param>
        public UriQuery(string query)
        {
            if (query != null)
            {
                int num = query.Length;
                for (int i = ((query.Length > 0) && (query[0] == '?')) ? 1 : 0; i < num; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        char ch = query[i];
                        if (ch == '=')
                        {
                            if (num4 < 0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string str = null;
                    string str2 = null;
                    if (num4 >= 0)
                    {
                        str = query.Substring(startIndex, num4 - startIndex);
                        str2 = query.Substring(num4 + 1, (i - num4) - 1);
                    }
                    else
                    {
                        str2 = query.Substring(startIndex, i - startIndex);
                    }

                    this.Add(str != null ? Uri.UnescapeDataString(str) : null, Uri.UnescapeDataString(str2));
                    if ((i == (num - 1)) && (query[i] == '&'))
                    {
                        this.Add(null, "");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <returns>The value for the specified key, or <see langword="null"/> if the query does not contain such a key.</returns>
        public string this[string key]
        {
            get
            {
                foreach (var kvp in this.entries)
                {
                    if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return kvp.Value;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            this.entries.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance as a query string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var queryBuilder = new StringBuilder();

            if (this.entries.Count > 0)
            {
                queryBuilder.Append('?');
                var first = true;

                foreach (var kvp in this.entries)
                {
                    if (!first)
                    {
                        queryBuilder.Append('&');
                    }
                    else
                    {
                        first = false;
                    }

                    queryBuilder.Append(Uri.EscapeDataString(kvp.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kvp.Value));
                }
            }

            return queryBuilder.ToString();

        }
    }
}
