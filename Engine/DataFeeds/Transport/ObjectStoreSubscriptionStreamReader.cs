/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using System.IO;

using QuantConnect.Interfaces;

namespace QuantConnect.Lean.Engine.DataFeeds.Transport
{
    /// <summary>
    /// Represents a stream reader capable of reading lines from the object store
    /// </summary>
    public class ObjectStoreSubscriptionStreamReader : IStreamReader
    {
        /// <summary>
        /// Gets whether or not this stream reader should be rate limited
        /// </summary>
        public bool ShouldBeRateLimited => false;

        /// <summary>
        /// Direct access to the StreamReader instance
        /// </summary>
        public StreamReader StreamReader { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectStoreSubscriptionStreamReader"/> class.
        /// </summary>
        /// <param name="objectStore">The <see cref="IObjectStore"/> used to retrieve a stream of data</param>
        /// <param name="key">The object store key the data should be fetched from</param>
        public ObjectStoreSubscriptionStreamReader(IObjectStore objectStore, string key)
        {
            if (objectStore.ContainsKey(key))
            {
                var data = objectStore.ReadBytes(key);
                var stream = new MemoryStream(data);
                StreamReader = new StreamReader(stream);
            }
        }

        /// <summary>
        /// Gets <see cref="SubscriptionTransportMedium.LocalFile"/>
        /// </summary>
        public SubscriptionTransportMedium TransportMedium
        {
            get { return SubscriptionTransportMedium.ObjectStore; }
        }

        /// <summary>
        /// Gets whether or not there's more data to be read in the stream
        /// </summary>
        public bool EndOfStream
        {
            get { return StreamReader == null || StreamReader.EndOfStream; }
        }

        /// <summary>
        /// Gets the next line/batch of content from the stream
        /// </summary>
        public string ReadLine()
        {
            return StreamReader.ReadLine();
        }

        /// <summary>
        /// Disposes of the stream
        /// </summary>
        public void Dispose()
        {
            if (StreamReader != null)
            {
                StreamReader.Dispose();
                StreamReader = null;
            }
        }
    }
}
