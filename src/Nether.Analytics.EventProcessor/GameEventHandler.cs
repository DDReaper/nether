using Nether.Analytics.EventProcessor.Output.Blob;
using Nether.Analytics.EventProcessor.Output.EventHub;
using Newtonsoft.Json.Linq;

namespace Nether.Analytics.EventProcessor
{
    /// <summary>
    /// Class that contains default Game Event Handling in Nether. The correct
    /// methods will be called by the GameEventRouter depending on GameEventType.
    /// Extend, override or replace this class and the contained actions in order
    /// to implement different game event handling in Nether.
    /// </summary>
    public class GameEventHandler
    {
        private readonly BlobOutputManager _blobOutputManager;
        private readonly EventHubOutputManager _eventHubOutputManager;

        public GameEventHandler(BlobOutputManager blobOutputManager, EventHubOutputManager eventHubOutputManager)
        {
            _blobOutputManager = blobOutputManager;
            _eventHubOutputManager = eventHubOutputManager;
        }

        #region GameEventActions
        public void HandleGameStartEvent(string gameEventType, string jsonEvent)
        {
            var csvEvent = jsonEvent.JsonToCsvString("type", "version", "clientUtcTime", "gameSessionId", "gamerTag");

            _blobOutputManager.SendTo(gameEventType, csvEvent);
        }

        public void HandleGameHeartbeat(string gameEventType, string jsonEvent)
        {
            var csvEvent = jsonEvent.JsonToCsvString("type", "version", "clientUtcTime", "gameSessionId");

            _blobOutputManager.SendTo(gameEventType, csvEvent);
            _eventHubOutputManager.SendTo(gameEventType, csvEvent);
        }
        #endregion

        /// <summary>
        /// Inspects gameEvent to figure out what GameEventType we are working with.
        /// This implementation assumes messages as sent as JSON with two properties
        /// "type" and "version".
        /// </summary>
        /// <param name="gameEvent">The JSON Game Event to inspect</param>
        /// <returns>The Game Event Type</returns>
        public static string ResolveEventType(string gameEvent)
        {
            var json = JObject.Parse(gameEvent);
            var gameEventType = (string)json["type"];
            var version = (string)json["version"];

            return VersionedName(gameEventType, version);
        }

        /// <summary>
        /// Combines Event Type and Version to a "versioned name"
        /// </summary>
        /// <param name="gameEventType">Game Event Type</param>
        /// <param name="version">Version of Game Event Type</param>
        /// <returns>A combined and versioned name</returns>
        public static string VersionedName(string gameEventType, string version)
        {
            return $"{gameEventType}|{version}";
        }


    }
}