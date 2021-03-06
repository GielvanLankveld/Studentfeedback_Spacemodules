﻿/**
 * Copyright (C) 2016 Open University (http://www.ou.nl/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StudentFeedback_SpaceModules
{
    class ElasticSearchQuery
    {

        private void RetrieveAllDataFromServer(string URL, string index)
        {
            /* This class requires a URL with an exposed ElasticSearch REST interface and an ElasticSearch index*/

            URL = "http://tracker.playgen.com/api/proxy/e5/";
            index = "5888fc78924fa4006e96b11e";
            string urlParameters = "/_search?size=5000&pretty=true&q=*:*";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL + index);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {

            }
        }

        private void RetrieveMostRecentDataFromServer(string URL, string index, int numberOfDocs)
        {

        }
        }
}
