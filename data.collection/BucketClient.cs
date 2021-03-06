﻿using System;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Amazon;
using System.Threading.Tasks;
using Carto.Core;

namespace data.collection
{
    public static class BucketClient
    {
        public static string Name = Conf.S3BucketName;

        public static string AccessKey = Conf.S3AccessKey;
        public static string SecretKey = Conf.S3SecretKey;

        public static string UploadPath = "https://" + Name + ".s3.amazonaws.com/";

        /* This is the base path, file name needs to be appended to this path, 
		 * e.g. https://s3.amazonaws.com/com.carto.mobile.images/test.jpg
         */
        public static string PublicReadPath = "https://s3.amazonaws.com/" + Name + "/";

        static IAmazonS3 client;

        public static async Task<BucketResponse> Upload(string filename, Stream stream)
        {
            BucketResponse response = new BucketResponse();

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = Name,
                Key = filename,
                CannedACL = S3CannedACL.PublicRead,
                InputStream = stream
            };

            PutObjectResponse intermediary = new PutObjectResponse();

            using (client = new AmazonS3Client(AccessKey, SecretKey, RegionEndpoint.USEast1))
            {
                try
                {
                    intermediary = await client.PutObjectAsync(request);
                    response.Message = "Image uploaded";
                    response.Path = PublicReadPath + filename;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                    response.Error = e.Message;
                }
            }

            return response;
        }

	}
}

