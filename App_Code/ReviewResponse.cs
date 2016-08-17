using Newtonsoft.Json;
using System.Collections.Generic;

/*
    *
    * JSON RESPONSE FORMAT
    * {
    *   "documents":[
    *       {
    *           "score":0.9404333,
    *           "id":"1"
    *       }
    *   ],
    *   "errors":[]
    * }
    *  
*/


namespace WhatNotToWatch.App_Code
{
    public class Document
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    [JsonObject]
    public class ReviewResponse
    {
        [JsonProperty(PropertyName = "documents")]
        public List<Document> documents { get; set; }
        public List<object> errors { get; set; }

        public double getScore()
        {
            if (documents != null && documents.Count >= 1)
            {
                return documents[0].score;
            }
            else return -1;
        }
    }
}
