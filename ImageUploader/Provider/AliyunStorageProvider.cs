﻿using Aliyun.OSS;
using System;
using System.IO;
using System.Xml.Linq;

namespace ImageUploader.Provider
{
    public class AliyunStorageProvider : IStorageProvider
    {
        private OssClient _client;

        public string Name { get; set; } = "Aliyun Oss";

        public string EndPoint { get; set; } = "YourEndPoint";

        public string AccessKeyId { get; set; } = "AccessKeyId";

        public string AccessKeySecret { get; set; } = "YourAccessKeySecret";

        public string BucketName { get; set; } = "YourBucketName";

        public OssClient Client
        {
            get
            {
                _client = _client ?? new OssClient(EndPoint, AccessKeyId, AccessKeySecret);
                return _client;
            }
        }

        public string GetUrl(string key)
            => $"http://{BucketName}.{EndPoint}/{key}";

        public bool Test()
            => Client.DoesBucketExist(BucketName);

        public bool Upload(string key, Stream stream)
        {
            var result = Client.PutObject(BucketName, key, stream);
            return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        #region [Xml Read & Write]

        public static AliyunStorageProvider FromXml(XElement element)
        {
            try
            {
                var provider = new AliyunStorageProvider()
                {
                    Name = element.Attribute(nameof(Name)).Value,
                    EndPoint = element.Attribute(nameof(EndPoint)).Value,
                    AccessKeyId = element.Attribute(nameof(AccessKeyId)).Value,
                    AccessKeySecret = element.Attribute(nameof(AccessKeySecret)).Value,
                    BucketName = element.Attribute(nameof(BucketName)).Value,
                };
                return provider;
            }
            catch(NullReferenceException)
            {
                return null;
            }
        }

        public XElement ToXml()
            => new XElement(nameof(AliyunStorageProvider),
                new XAttribute(nameof(Name), Name),
                new XAttribute(nameof(EndPoint), EndPoint),
                new XAttribute(nameof(AccessKeyId), AccessKeyId),
                new XAttribute(nameof(AccessKeySecret), AccessKeySecret),
                new XAttribute(nameof(BucketName), BucketName)
                );

        #endregion
    }
}