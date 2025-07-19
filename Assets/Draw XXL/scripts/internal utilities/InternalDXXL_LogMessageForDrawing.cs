namespace DrawXXL
{

    using UnityEngine;


    public class InternalDXXL_LogMessageForDrawing
    {
        //from Debug.Log()-Call:
        public string logString;
        public string stackTrace;
        public LogType logType;
        public int gameObjectsInstanceID;
        //from LogsAtGameObject()-Parsing:
        public string stringWithLogSymbolStackTraceAndLineBreak;
        public string wholeTextWallForLogDisplayOfLastXLogs;
    }

}
