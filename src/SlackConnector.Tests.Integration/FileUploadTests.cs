﻿using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SlackConnector.Tests.Integration.Configuration;
using SlackConnector.Tests.Integration.Resources;

namespace SlackConnector.Tests.Integration
{
    [TestFixture]
    public class FileUploadTests
    {
        private ISlackConnection _slackConnection;
        private Config _config;
        private string _filePath;

        [SetUp]
        public void SetUp()
        {
            _filePath = Path.GetTempFileName();
            File.WriteAllText(_filePath, EmbeddedResourceFileReader.ReadEmbeddedFileAsText("UploadTest.txt"));

            _config = new ConfigReader().GetConfig();
            var slackConnector = new SlackConnector();
            _slackConnection = slackConnector.Connect(_config.Slack.ApiToken).Result;
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_filePath);
        }

        [Test]
        public async Task should_upload_to_channel_from_file_system()
        {
            // given
            var chatHub = _slackConnection.ConnectedChannel(_config.Slack.TestChannel);

            // when
            await _slackConnection.Upload(chatHub, _filePath);

            // then
        }

        [Test]
        public async Task should_upload_to_channel_from_stream()
        {
            // given
            var chatHub = _slackConnection.ConnectedChannel(_config.Slack.TestChannel);
            const string fileName = "slackconnector-test-stream-upload.txt";

            // when
            using (var fileStream = EmbeddedResourceFileReader.ReadEmbeddedFile("UploadTest.txt"))
            {
                await _slackConnection.Upload(chatHub, fileStream, fileName);
            }

            // then
        }

    }
}
