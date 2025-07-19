namespace DrawXXL
{
    using System;
    using System.Diagnostics;
    using UnityEngine;
    using System.Collections.Generic;

    public class DrawLogs
    {
        public static bool autoMarkupLogTextWithGameobjectColor_forConsole = false;
        public static bool autoMarkupLogTextWithGameobjectColor_forDrawingInScene = false;
        public static bool autoMarkupLogTextWithGameobjectColor_forDrawingInScrenspace = false;
        public static float forceLuminance_ofAutoMarkupColors = 0.0f;

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Assert(condition, message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            if (condition == false)
            {
                NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context, message);
            }
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition)
        {
            UnityEngine.Debug.Assert(condition);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString = GetStringFromObject(message);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message_asString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Assert(condition, message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            if (condition == false)
            {
                NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context, message);
            }
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(assertionFailedString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Assert(condition, message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.Assert(condition, context);
            }
            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            if (condition == false)
            {
                NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context);
            }
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromFormatAndArgs(format, args);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Assert(condition, message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.AssertFormat(condition, context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            if (condition == false)
            {
                NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context, format, args);
            }
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, format, args);
        }

        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        public static void Log(object message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString = GetStringFromObject(message);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message_asString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Log(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.Log(message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Log, context, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString = GetStringFromObject(message);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message_asString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogAssertion(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogAssertion(message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context, message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(message);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromFormatAndArgs(format, args);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogAssertion(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogAssertionFormat(context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Assert, context, format, args);
        }

        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(format, args);
        }

        public static void LogError(object message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString = GetStringFromObject(message);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message_asString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogError(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogError(message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Error, context, message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public static void LogErrorFormat(GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromFormatAndArgs(format, args);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogError(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogErrorFormat(context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Error, context, format, args);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }

        public static void LogException(System.Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        public static void LogException(System.Exception exception, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromException(exception);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogError(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogException(exception, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Exception, context, exception);
        }

        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }

        public static void LogFormat(GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromFormatAndArgs(format, args);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.Log(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogFormat(context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Log, context, format, args);
        }

        public static void LogFormat(LogType logType, LogOption logOptions, GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string format_withColorMarkup = DrawText.MarkupColorFromGameobjectID(format, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogFormat(logType, logOptions, context, format_withColorMarkup, args);
            }
            else
            {
                UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(logType, context, format, args);
        }

        public static void LogWarning(object message, GameObject context)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string message_asString = GetStringFromObject(message);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(message_asString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogWarning(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogWarning(message, context);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Warning, context, message);
        }

        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public static void LogWarningFormat(GameObject context, string format, params object[] args)
        {
            if (autoMarkupLogTextWithGameobjectColor_forConsole)
            {
                string messageAsString = GetStringFromFormatAndArgs(format, args);
                string message_asString_withColorMarkup = DrawText.MarkupColorFromGameobjectID(messageAsString, context, forceLuminance_ofAutoMarkupColors);
                UnityEngine.Debug.LogWarning(message_asString_withColorMarkup, context);
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat(context, format, args);
            }

            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }
            NoteLogMessageForDrawingAtGameObjects(LogType.Warning, context, format, args);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }

        static void NoteLogMessageForDrawingAtGameObjects(LogType logType, GameObject gameObject_toWhichLogMessageBelongs, object message)
        {
            string messageString = GetStringFromObject(message, ref logType);
            NoteLogMessageForDrawingAtGameObjects(logType, gameObject_toWhichLogMessageBelongs, messageString);
        }

        static string GetStringFromObject(object message)
        {
            LogType dummyLogType = default;
            return GetStringFromObject(message, ref dummyLogType);
        }

        static string GetStringFromObject(object message, ref LogType logType)
        {
            if (message == null)
            {
                return "null";
            }
            else
            {
                string messageString;
                try
                {
                    messageString = message.ToString();
                }
                catch (Exception exception)
                {
                    try
                    {
                        messageString = "[" + DrawText.MarkupLogSymbol(logType) + " -> " + DrawText.MarkupLogSymbol(LogType.Exception) + "] Converting message object to string threw an exception: " + exception.GetType() + ": " + exception.Message;
                    }
                    catch
                    {
                        messageString = "Converting message object to string threw an exception, and parsing this exception to string again threw another exception.";
                    }
                    logType = LogType.Exception;
                }
                return messageString;
            }
        }

        static string assertionFailedString = "Assertion failed";
        static void NoteLogMessageForDrawingAtGameObjects(LogType logType, GameObject gameObject_toWhichLogMessageBelongs)
        {
            NoteLogMessageForDrawingAtGameObjects(logType, gameObject_toWhichLogMessageBelongs, assertionFailedString);
        }

        static void NoteLogMessageForDrawingAtGameObjects(LogType logType, GameObject gameObject_toWhichLogMessageBelongs, string format, params object[] args)
        {
            string messageString = GetStringFromFormatAndArgs(ref logType, format, args);
            NoteLogMessageForDrawingAtGameObjects(logType, gameObject_toWhichLogMessageBelongs, messageString);
        }

        static string GetStringFromFormatAndArgs(string format, params object[] args)
        {
            LogType dummyLogType = default;
            return GetStringFromFormatAndArgs(ref dummyLogType, format, args);
        }

        static string GetStringFromFormatAndArgs(ref LogType logType, string format, params object[] args)
        {
            if (format == null)
            {
                return "null";
            }
            else
            {
                string messageString;
                try
                {
                    messageString = String.Format(format, args);
                }
                catch (Exception exception)
                {
                    try
                    {
                        messageString = "[" + DrawText.MarkupLogSymbol(logType) + " -> " + DrawText.MarkupLogSymbol(LogType.Exception) + "] Formatting the logString threw an exception: " + exception.GetType() + ": " + exception.Message;
                    }
                    catch
                    {
                        messageString = "Formatting the logString threw an exception, and parsing this exception to string again threw another exception.";
                    }
                    logType = LogType.Exception;
                }
                return messageString;
            }
        }

        static void NoteLogMessageForDrawingAtGameObjects(LogType logType, GameObject gameObject_toWhichLogMessageBelongs, System.Exception exception)
        {
            string message = GetStringFromException(exception);
            NoteLogMessageForDrawingAtGameObjects(logType, gameObject_toWhichLogMessageBelongs, message, exception.StackTrace);
        }

        static string GetStringFromException(System.Exception exception)
        {
            string message;
            try
            {
                message = "" + exception.GetType() + ": " + exception.Message;
            }
            catch (Exception)
            {
                message = "LogException, that failed to parse to string.";
            }
            return message;
        }

        static void NoteLogMessageForDrawingAtGameObjects(LogType logType, GameObject gameObject_toWhichLogMessageBelongs, string message, string stackTrace = null)
        {
            if (gameObject_toWhichLogMessageBelongs != null)
            {
                if (message != null)
                {
                    InternalDXXL_LogMessageForDrawing receivedLogMessage = new InternalDXXL_LogMessageForDrawing();
                    receivedLogMessage.logString = message;
                    receivedLogMessage.stackTrace = stackTrace;
                    receivedLogMessage.logType = logType;
                    receivedLogMessage.gameObjectsInstanceID = gameObject_toWhichLogMessageBelongs.GetInstanceID();
                    logMessagesForDrawAtGameObjects.Add(receivedLogMessage);
                }
            }
        }

        public static void LogsAtGameObject(GameObject gameObject, bool drawNormalPrio = true, bool drawWarningPrio = true, bool drawErrorPrio = true, int maxNumberOfDisplayedLogMessages = 10, float textSize = 0.2f, Color textColor = default(Color), Color boxColor = default(Color), float width_ofBoxLines = 0.0f, bool drawnBoxEncapsulatesChildren = true, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //if you supply a different "maxNumberOfDisplayedLogMessages" or "draw*Prio" during runtime: Then this comes into effect not immediately, but after a delay, namely when a new log message gets noted. Reason: optimization of GC allocations.
            //the same goes if you change during runtime between "LogsAtGameObject" and "LogsAtGameObjectScreenSpace"

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSize, "textSize")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBoxLines, "width_ofBoxLines")) { return; }

            if (maxNumberOfDisplayedLogMessages > maxMaxNumberOfNumberOfLogDisplayerLogMessages)
            {
                UnityEngine.Debug.Log("The maximum allowed value for 'maxNumberOfDisplayedLogMessages' is " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + " -> Auto-force from " + maxNumberOfDisplayedLogMessages + " to " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + "");
                maxNumberOfDisplayedLogMessages = maxMaxNumberOfNumberOfLogDisplayerLogMessages;
            }

            textColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(textColor);
            if (autoMarkupLogTextWithGameobjectColor_forDrawingInScene) { textColor = SeededColorGenerator.ColorOfGameobjectID(gameObject, forceLuminance_ofAutoMarkupColors); }
            string logsAsTextWall = GetStringWithXNewestLogsForGameObject(gameObject, maxNumberOfDisplayedLogMessages, drawNormalPrio, drawWarningPrio, drawErrorPrio);
            DrawEngineBasics.TagGameObject(gameObject, logsAsTextWall, textColor, boxColor, textSize, width_ofBoxLines, drawnBoxEncapsulatesChildren, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void LogsAtGameObjectScreenspace(GameObject gameObject, bool drawNormalPrio = true, bool drawWarningPrio = true, bool drawErrorPrio = true, bool clampIntoScreen = true, int maxNumberOfDisplayedLogMessages = 10, float relTextSizeScaling = 1.0f, Color textColor = default(Color), Color boxColor = default(Color), float widthOfBoxLines_relToViewportHeight = 0.0f, bool drawnBoxEncapsulatesChildren = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawLogs.LogsAtGameObjectScreenspace") == false) { return; }
            LogsAtGameObjectScreenspace(automaticallyFoundCamera, gameObject, drawNormalPrio, drawWarningPrio, drawErrorPrio, clampIntoScreen, maxNumberOfDisplayedLogMessages, relTextSizeScaling, textColor, boxColor, widthOfBoxLines_relToViewportHeight, drawnBoxEncapsulatesChildren, durationInSec);
        }

        public static void LogsAtGameObjectScreenspace(Camera cameraWhereToDraw, GameObject gameObject, bool drawNormalPrio = true, bool drawWarningPrio = true, bool drawErrorPrio = true, bool clampIntoScreen = true, int maxNumberOfDisplayedLogMessages = 10, float relTextSizeScaling = 1.0f, Color textColor = default(Color), Color boxColor = default(Color), float widthOfBoxLines_relToViewportHeight = 0.0f, bool drawnBoxEncapsulatesChildren = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(cameraWhereToDraw, "cameraWhereToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(widthOfBoxLines_relToViewportHeight, "widthOfBoxLines_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(relTextSizeScaling, "relTextSizeScaling")) { return; }

            if (maxNumberOfDisplayedLogMessages > maxMaxNumberOfNumberOfLogDisplayerLogMessages)
            {
                UnityEngine.Debug.Log("The maximum allowed value for 'maxNumberOfDisplayedLogMessages' is " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + " -> Auto-force from " + maxNumberOfDisplayedLogMessages + " to " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + "");
                maxNumberOfDisplayedLogMessages = maxMaxNumberOfNumberOfLogDisplayerLogMessages;
            }

            textColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(textColor);
            if (autoMarkupLogTextWithGameobjectColor_forDrawingInScrenspace) { textColor = SeededColorGenerator.ColorOfGameobjectID(gameObject, forceLuminance_ofAutoMarkupColors); }
            string logsAsTextWall = GetStringWithXNewestLogsForGameObject(gameObject, maxNumberOfDisplayedLogMessages, drawNormalPrio, drawWarningPrio, drawErrorPrio);
            DrawEngineBasics.TagGameObjectScreenspace(cameraWhereToDraw, gameObject, logsAsTextWall, textColor, boxColor, widthOfBoxLines_relToViewportHeight, clampIntoScreen, 0.6f * relTextSizeScaling, drawnBoxEncapsulatesChildren, durationInSec);
        }

        public static void LogsOnScreen(bool drawNormalPrio = true, bool drawWarningPrio = true, bool drawErrorPrio = true, int maxNumberOfDisplayedLogMessages = 10, float textSize_relToViewportHeight = 0.014f, Color textColor = default(Color), bool stackTraceForNormalPrio = false, bool stackTraceForWarningPrio = false, bool stackTraceForErrorPrio = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawLogs.LogsOnScreen") == false) { return; }
            LogsOnScreen(automaticallyFoundCamera, drawNormalPrio, drawWarningPrio, drawErrorPrio, maxNumberOfDisplayedLogMessages, textSize_relToViewportHeight, textColor, stackTraceForNormalPrio, stackTraceForWarningPrio, stackTraceForErrorPrio, durationInSec);
        }

        public static void LogsOnScreen(Camera cameraWhereToDraw, bool drawNormalPrio = true, bool drawWarningPrio = true, bool drawErrorPrio = true, int maxNumberOfDisplayedLogMessages = 10, float textSize_relToViewportHeight = 0.014f, Color textColor = default(Color), bool stackTraceForNormalPrio = false, bool stackTraceForWarningPrio = false, bool stackTraceForErrorPrio = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(cameraWhereToDraw, "cameraWhereToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSize_relToViewportHeight, "textSize_relToViewportHeight")) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_LogsOnScreen.Add(new LogsOnScreen(cameraWhereToDraw, drawNormalPrio, drawWarningPrio, drawErrorPrio, maxNumberOfDisplayedLogMessages, textSize_relToViewportHeight, textColor, stackTraceForNormalPrio, stackTraceForWarningPrio, stackTraceForErrorPrio, durationInSec, logMessageListenerForLogsOnScreen_isActivated));
                return;
            }

            if (maxNumberOfDisplayedLogMessages > maxMaxNumberOfNumberOfLogDisplayerLogMessages)
            {
                UnityEngine.Debug.Log("The maximum allowed value for 'maxNumberOfDisplayedLogMessages' is " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + " -> Auto-force from " + maxNumberOfDisplayedLogMessages + " to " + maxMaxNumberOfNumberOfLogDisplayerLogMessages + "");
                maxNumberOfDisplayedLogMessages = maxMaxNumberOfNumberOfLogDisplayerLogMessages;
            }

            textColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(textColor);
            string logsAsTextWall = GetStringWithXNewestLogsForDrawOnScreen(maxNumberOfDisplayedLogMessages, drawNormalPrio, drawWarningPrio, drawErrorPrio, stackTraceForNormalPrio, stackTraceForWarningPrio, stackTraceForErrorPrio);
            UtilitiesDXXL_Text.WriteScreenspace(cameraWhereToDraw, logsAsTextWall, new Vector2(textSize_relToViewportHeight, 0.0f), textColor, textSize_relToViewportHeight, 0.0f, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
        }

        static List<InternalDXXL_LogMessageForDrawing> logMessagesForDrawAtGameObjects = new List<InternalDXXL_LogMessageForDrawing>();
        static int maxMaxNumberOfNumberOfLogDisplayerLogMessages = 40; //has to be dividable by "4" //if value is raised: danger of performance issues/freezing 
        static int maxNumberOfBlocksOf4ConcattedStrings = Mathf.RoundToInt(0.25f * maxMaxNumberOfNumberOfLogDisplayerLogMessages);
        static string[] blocksOf4ConcattedStrings_startingWithNewest = new string[maxNumberOfBlocksOf4ConcattedStrings];
        static int[] indexes_ofCurrLog_insideAllLogsList_startingWithNewest = new int[maxMaxNumberOfNumberOfLogDisplayerLogMessages];
        static string GetStringWithXNewestLogsForGameObject(GameObject gameObject, int maxNumberOfDisplayedLogMessages, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio)
        {
            if (getNormalPrio == false && getWarningPrio == false && getErrorPrio == false)
            {
                return "(all logTypes are disabled)";
            }

            maxNumberOfDisplayedLogMessages = Mathf.Abs(maxNumberOfDisplayedLogMessages);
            maxNumberOfDisplayedLogMessages = Mathf.Max(maxNumberOfDisplayedLogMessages, 1);
            maxNumberOfDisplayedLogMessages = Mathf.Min(maxNumberOfDisplayedLogMessages, maxMaxNumberOfNumberOfLogDisplayerLogMessages);

            int gameObjectsInstanceId = gameObject.GetInstanceID();

            int numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject = 0;
            int numberOf_savedNormalLogMessagesThatAreAttachedToGameObject = 0;
            int numberOf_savedWarningLogMessagesThatAreAttachedToGameObject = 0;
            int numberOf_savedErrorLogMessagesThatAreAttachedToGameObject = 0;
            for (int i = 0; i < logMessagesForDrawAtGameObjects.Count; i++)
            {
                if (gameObjectsInstanceId == logMessagesForDrawAtGameObjects[i].gameObjectsInstanceID)
                {
                    if (logMessagesForDrawAtGameObjects[i].logType == LogType.Log)
                    {
                        numberOf_savedNormalLogMessagesThatAreAttachedToGameObject++;
                        if (getNormalPrio)
                        {
                            numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject++;
                        }
                    }
                    else
                    {
                        if (logMessagesForDrawAtGameObjects[i].logType == LogType.Warning)
                        {
                            numberOf_savedWarningLogMessagesThatAreAttachedToGameObject++;
                            if (getWarningPrio)
                            {
                                numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject++;
                            }
                        }
                        else
                        {
                            //"exception" and "assertion" count as "error":
                            numberOf_savedErrorLogMessagesThatAreAttachedToGameObject++;
                            if (getErrorPrio)
                            {
                                numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject++;
                            }
                        }
                    }
                }
            }

            if (numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject == 0)
            {
                return "(no log messages yet for the requested log types)";
            }
            else
            {
                int numberOfMessagesInTextWall = FillGameObjectsConcerned_stringsWithLogSymbolStackTraceAndLineBreak(gameObjectsInstanceId, maxNumberOfDisplayedLogMessages, getNormalPrio, getWarningPrio, getErrorPrio);
                return GetWholeLogTextWall(ref logMessagesForDrawAtGameObjects, numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject, numberOfMessagesInTextWall, numberOf_savedNormalLogMessagesThatAreAttachedToGameObject, numberOf_savedWarningLogMessagesThatAreAttachedToGameObject, numberOf_savedErrorLogMessagesThatAreAttachedToGameObject, getNormalPrio, getWarningPrio, getErrorPrio);
            }
        }

        static int FillGameObjectsConcerned_stringsWithLogSymbolStackTraceAndLineBreak(int gameObjectsInstanceId, int maxNumberOfDisplayedLogMessages, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio)
        {
            int numberOfAlreadyRetrievedMessages = 0;
            for (int i_logsOfAllGameObjects = logMessagesForDrawAtGameObjects.Count - 1; i_logsOfAllGameObjects >= 0; i_logsOfAllGameObjects--)
            {
                if (gameObjectsInstanceId == logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].gameObjectsInstanceID)
                {
                    if (IsARequestedLogType(logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].logType, getNormalPrio, getWarningPrio, getErrorPrio))
                    {
                        if (logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stringWithLogSymbolStackTraceAndLineBreak == null)
                        {
                            logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stringWithLogSymbolStackTraceAndLineBreak = DrawText.MarkupLogSymbol(logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].logType) + " " + logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].logString + "<br>";
                            if (logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stackTrace != null)
                            {
                                logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stringWithLogSymbolStackTraceAndLineBreak = logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stringWithLogSymbolStackTraceAndLineBreak + logMessagesForDrawAtGameObjects[i_logsOfAllGameObjects].stackTrace + "<br>";
                            }
                        }

                        indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyRetrievedMessages] = i_logsOfAllGameObjects;
                        numberOfAlreadyRetrievedMessages++;
                    }
                }
                if (numberOfAlreadyRetrievedMessages >= maxNumberOfDisplayedLogMessages) { break; }
            }
            return numberOfAlreadyRetrievedMessages;
        }

        static bool IsARequestedLogType(LogType logTypeToCheckIfRequested, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio)
        {
            if (logTypeToCheckIfRequested == LogType.Log)
            {
                if (getNormalPrio)
                {
                    return true;
                }
            }
            else
            {
                if (logTypeToCheckIfRequested == LogType.Warning)
                {
                    if (getWarningPrio)
                    {
                        return true;
                    }
                }
                else
                {
                    //"exception" and "assertion" count as "error":
                    if (getErrorPrio)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static string GetWholeLogTextWall(ref List<InternalDXXL_LogMessageForDrawing> listOfSavedLogMessages, int numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject, int numberOfMessagesInTextWall, int numberOf_savedNormalLogMessagesThatAreAttachedToGameObject, int numberOf_savedWarningLogMessagesThatAreAttachedToGameObject, int numberOf_savedErrorLogMessagesThatAreAttachedToGameObject, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio)
        {
            if (listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs == null)
            {
                string headerLine = GetHeaderLineString(numberOfSavedRequestedTypesMessagesThatAreAttachedToGameObject, numberOfMessagesInTextWall, numberOf_savedNormalLogMessagesThatAreAttachedToGameObject, numberOf_savedWarningLogMessagesThatAreAttachedToGameObject, numberOf_savedErrorLogMessagesThatAreAttachedToGameObject, getNormalPrio, getWarningPrio, getErrorPrio);

                int numberOfAlreadyConcattedMessages = 0;
                int numberOfUsedSlots_inConcatted4StringsArray = 0;
                bool allSlotsInStringOf4ArrayFilled = false;

                //trading code readability for GC.Alloc()-prevention:
                for (int i_concatted4Strings = 0; i_concatted4Strings < maxNumberOfBlocksOf4ConcattedStrings; i_concatted4Strings++)
                {
                    int numberOfLogsForNext_blockOf4ConcattedStrings = (numberOfAlreadyConcattedMessages + 4) <= numberOfMessagesInTextWall ? 4 : (numberOfMessagesInTextWall - numberOfAlreadyConcattedMessages);
                    switch (numberOfLogsForNext_blockOf4ConcattedStrings)
                    {
                        case 0:
                            blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = headerLine;
                            allSlotsInStringOf4ArrayFilled = true;
                            break;

                        case 1:
                            blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = headerLine + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages]].stringWithLogSymbolStackTraceAndLineBreak;
                            allSlotsInStringOf4ArrayFilled = true;
                            break;

                        case 2:
                            blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = headerLine + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 1]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages]].stringWithLogSymbolStackTraceAndLineBreak;
                            allSlotsInStringOf4ArrayFilled = true;
                            break;

                        case 3:
                            blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = headerLine + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 2]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 1]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages]].stringWithLogSymbolStackTraceAndLineBreak;
                            allSlotsInStringOf4ArrayFilled = true;
                            break;

                        case 4:
                            blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 3]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 2]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages + 1]].stringWithLogSymbolStackTraceAndLineBreak + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyConcattedMessages]].stringWithLogSymbolStackTraceAndLineBreak;
                            numberOfAlreadyConcattedMessages = numberOfAlreadyConcattedMessages + 4;
                            break;

                        default:
                            UtilitiesDXXL_Log.PrintErrorCode("1-" + numberOfLogsForNext_blockOf4ConcattedStrings);
                            break;
                    }

                    if (allSlotsInStringOf4ArrayFilled)
                    {
                        numberOfUsedSlots_inConcatted4StringsArray = i_concatted4Strings + 1;
                        break;
                    }

                    if (i_concatted4Strings >= (maxNumberOfBlocksOf4ConcattedStrings - 1))
                    {
                        //->all slots are filled
                        blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings] = headerLine + blocksOf4ConcattedStrings_startingWithNewest[i_concatted4Strings];
                        numberOfUsedSlots_inConcatted4StringsArray = i_concatted4Strings + 1;
                        break;
                    }

                }

                switch (numberOfUsedSlots_inConcatted4StringsArray)
                {
                    case 0:
                        UtilitiesDXXL_Log.PrintErrorCode("2");
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = null;
                        break;

                    case 1:
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[0];
                        break;

                    case 2:
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[1] + blocksOf4ConcattedStrings_startingWithNewest[0];
                        break;

                    case 3:
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[2] + blocksOf4ConcattedStrings_startingWithNewest[1] + blocksOf4ConcattedStrings_startingWithNewest[0];
                        break;

                    case 4:
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[3] + blocksOf4ConcattedStrings_startingWithNewest[2] + blocksOf4ConcattedStrings_startingWithNewest[1] + blocksOf4ConcattedStrings_startingWithNewest[0];
                        break;

                    default:
                        listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[3] + blocksOf4ConcattedStrings_startingWithNewest[2] + blocksOf4ConcattedStrings_startingWithNewest[1] + blocksOf4ConcattedStrings_startingWithNewest[0];
                        //no GC.Alloc()-optimization for more than 14 maxNumberOfDisplayedLogMessages:
                        for (int i = 4; i < numberOfUsedSlots_inConcatted4StringsArray; i++)
                        {
                            listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs = blocksOf4ConcattedStrings_startingWithNewest[i] + listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs;
                        }
                        break;
                }
            }
            return listOfSavedLogMessages[indexes_ofCurrLog_insideAllLogsList_startingWithNewest[0]].wholeTextWallForLogDisplayOfLastXLogs;
        }

        static string GetHeaderLineString(int overallNumberOfSavedRequestedTypesMessages, int numberOfMessagesInTextWall, int numberOf_normalLogMessages, int numberOf_warningLogMessages, int numberOf_errorLogMessages, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio)
        {
            //trading code readability for GC.Alloc()-prevention:
            if (getNormalPrio)
            {
                if (getWarningPrio)
                {
                    if (getErrorPrio)
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: " + numberOf_normalLogMessages + "<size=4> </size><color=#adadadFF><icon=logMessage></color>, " + numberOf_warningLogMessages + "<size=4> </size><color=#e2aa00FF><icon=warning></color>, " + numberOf_errorLogMessages + "<size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                    else
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: " + numberOf_normalLogMessages + "<size=4> </size><color=#adadadFF><icon=logMessage></color>, " + numberOf_warningLogMessages + "<size=4> </size><color=#e2aa00FF><icon=warning></color>, <d>" + numberOf_errorLogMessages + "</d><size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                }
                else
                {
                    if (getErrorPrio)
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: " + numberOf_normalLogMessages + "<size=4> </size><color=#adadadFF><icon=logMessage></color>, <d>" + numberOf_warningLogMessages + "</d><size=4> </size><color=#e2aa00FF><icon=warning></color>, " + numberOf_errorLogMessages + "<size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                    else
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: " + numberOf_normalLogMessages + "<size=4> </size><color=#adadadFF><icon=logMessage></color>, <d>" + numberOf_warningLogMessages + "</d><size=4> </size><color=#e2aa00FF><icon=warning></color>, <d>" + numberOf_errorLogMessages + "</d><size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                }
            }
            else
            {
                if (getWarningPrio)
                {
                    if (getErrorPrio)
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: <d>" + numberOf_normalLogMessages + "</d><size=4> </size><color=#adadadFF><icon=logMessage></color>, " + numberOf_warningLogMessages + "<size=4> </size><color=#e2aa00FF><icon=warning></color>, " + numberOf_errorLogMessages + "<size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                    else
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: <d>" + numberOf_normalLogMessages + "</d><size=4> </size><color=#adadadFF><icon=logMessage></color>, " + numberOf_warningLogMessages + "<size=4> </size><color=#e2aa00FF><icon=warning></color>, <d>" + numberOf_errorLogMessages + "</d><size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                }
                else
                {
                    if (getErrorPrio)
                    {
                        return ("<icon=left_twoStroke> ..." + (overallNumberOfSavedRequestedTypesMessages - numberOfMessagesInTextWall) + " more message(s) (overall: <d>" + numberOf_normalLogMessages + "</d><size=4> </size><color=#adadadFF><icon=logMessage></color>, <d>" + numberOf_warningLogMessages + "</d><size=4> </size><color=#e2aa00FF><icon=warning></color>, " + numberOf_errorLogMessages + "<size=4> </size><color=#ce0e0eFF><icon=logMessageError></color>)<br>");
                    }
                    else
                    {
                        return "(all logTypes are disabled)";
                    }
                }
            }
        }

        public static void ClearLogs(bool clearLogsForDrawingAtGameobjects = true, bool clearLogsForDrawingToScreenspace = true)
        {
            if (clearLogsForDrawingAtGameobjects)
            {
                logMessagesForDrawAtGameObjects = new List<InternalDXXL_LogMessageForDrawing>();
            }

            if (clearLogsForDrawingToScreenspace)
            {
                logMessagesForDrawnOnScreen = new List<InternalDXXL_LogMessageForDrawing>();
            }
        }

        private static bool logMessageListenerForLogsOnScreen_isActivated = false;
        public static bool LogMessageListenerForLogsOnScreen_isActivated
        {
            get { return logMessageListenerForLogsOnScreen_isActivated; }
            set { UnityEngine.Debug.LogError("Don't set 'LogMessageListenerForLogsOnScreen_isActivated' manually. Use 'ActivateLogMessageListenerForLogsOnScreen()' or 'DecactivateLogMessageListenerForLogsOnScreen()' instead."); }
        }

        public static void ActivateLogMessageListenerForLogsOnScreen()
        {
            Application.logMessageReceived -= SaveLogForDrawingOnScreen;
            Application.logMessageReceived += SaveLogForDrawingOnScreen;
            logMessageListenerForLogsOnScreen_isActivated = true;
        }

        public static void DeactivateLogMessageListenerForLogsOnScreen()
        {
            Application.logMessageReceived -= SaveLogForDrawingOnScreen;
            logMessageListenerForLogsOnScreen_isActivated = false;
        }

        static void SaveLogForDrawingOnScreen(string logString, string stackTrace, LogType type)
        {
            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled) { return; }

            InternalDXXL_LogMessageForDrawing receivedLogMessage = new InternalDXXL_LogMessageForDrawing();
            receivedLogMessage.logString = logString;
            receivedLogMessage.stackTrace = stackTrace;
            receivedLogMessage.logType = type;
            logMessagesForDrawnOnScreen.Add(receivedLogMessage);
        }

        static List<InternalDXXL_LogMessageForDrawing> logMessagesForDrawnOnScreen = new List<InternalDXXL_LogMessageForDrawing>();
        static string GetStringWithXNewestLogsForDrawOnScreen(int maxNumberOfDisplayedLogMessages, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio, bool stackTraceForNormalPrio, bool stackTraceForWarningPrio, bool stackTraceForErrorPrio)
        {
            if (logMessageListenerForLogsOnScreen_isActivated == false)
            {
                return "In order to use 'DrawLogs.LogsOnScreen()' you have activate it by calling<br>'DrawLogs.ActivateLogMessageListenerForLogsOnScreen()' once beforehand.";
            }

            if (getNormalPrio == false && getWarningPrio == false && getErrorPrio == false)
            {
                return "(all logTypes are disabled)";
            }

            maxNumberOfDisplayedLogMessages = Mathf.Abs(maxNumberOfDisplayedLogMessages);
            maxNumberOfDisplayedLogMessages = Mathf.Max(maxNumberOfDisplayedLogMessages, 1);
            maxNumberOfDisplayedLogMessages = Mathf.Min(maxNumberOfDisplayedLogMessages, maxMaxNumberOfNumberOfLogDisplayerLogMessages);

            int numberOfSavedRequestedTypesMessages = 0;
            int numberOf_savedNormalLogMessages = 0;
            int numberOf_savedWarningLogMessages = 0;
            int numberOf_savedErrorLogMessages = 0;
            for (int i = 0; i < logMessagesForDrawnOnScreen.Count; i++)
            {
                if (logMessagesForDrawnOnScreen[i].logType == LogType.Log)
                {
                    numberOf_savedNormalLogMessages++;
                    if (getNormalPrio)
                    {
                        numberOfSavedRequestedTypesMessages++;
                    }
                }
                else
                {
                    if (logMessagesForDrawnOnScreen[i].logType == LogType.Warning)
                    {
                        numberOf_savedWarningLogMessages++;
                        if (getWarningPrio)
                        {
                            numberOfSavedRequestedTypesMessages++;
                        }
                    }
                    else
                    {
                        //"exception" and "assertion" count as "error":
                        numberOf_savedErrorLogMessages++;
                        if (getErrorPrio)
                        {
                            numberOfSavedRequestedTypesMessages++;
                        }
                    }
                }
            }

            if (numberOfSavedRequestedTypesMessages == 0)
            {
                return "(no log messages yet for the requested log types)";
            }
            else
            {
                int numberOfMessagesInTextWall = FillConcerned_stringsWithLogSymbolStackTraceAndLineBreak(maxNumberOfDisplayedLogMessages, getNormalPrio, getWarningPrio, getErrorPrio, stackTraceForNormalPrio, stackTraceForWarningPrio, stackTraceForErrorPrio);
                return GetWholeLogTextWall(ref logMessagesForDrawnOnScreen, numberOfSavedRequestedTypesMessages, numberOfMessagesInTextWall, numberOf_savedNormalLogMessages, numberOf_savedWarningLogMessages, numberOf_savedErrorLogMessages, getNormalPrio, getWarningPrio, getErrorPrio);
            }
        }

        static int FillConcerned_stringsWithLogSymbolStackTraceAndLineBreak(int maxNumberOfDisplayedLogMessages, bool getNormalPrio, bool getWarningPrio, bool getErrorPrio, bool stackTraceForNormalPrio, bool stackTraceForWarningPrio, bool stackTraceForErrorPrio)
        {
            int numberOfAlreadyRetrievedMessages = 0;
            for (int i_ofAllSavedLogs = logMessagesForDrawnOnScreen.Count - 1; i_ofAllSavedLogs >= 0; i_ofAllSavedLogs--)
            {
                if (IsARequestedLogType(logMessagesForDrawnOnScreen[i_ofAllSavedLogs].logType, getNormalPrio, getWarningPrio, getErrorPrio))
                {
                    if (logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stringWithLogSymbolStackTraceAndLineBreak == null)
                    {
                        logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stringWithLogSymbolStackTraceAndLineBreak = DrawText.MarkupLogSymbol(logMessagesForDrawnOnScreen[i_ofAllSavedLogs].logType) + " " + logMessagesForDrawnOnScreen[i_ofAllSavedLogs].logString + "<br>";
                        if (logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stackTrace != null)
                        {
                            if (IsARequestedLogType(logMessagesForDrawnOnScreen[i_ofAllSavedLogs].logType, stackTraceForNormalPrio, stackTraceForWarningPrio, stackTraceForErrorPrio))
                            {
                                //-> it is not trivial here to simply indent the stack trace, because the ".stackTrace" field comes already with inserted line breaks, so the line start positions are not directly available here
                                logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stringWithLogSymbolStackTraceAndLineBreak = logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stringWithLogSymbolStackTraceAndLineBreak + "<size=6> </size><br><size=6>-----------------------------[STACK TRACE]-----------------------------</size><br>" + logMessagesForDrawnOnScreen[i_ofAllSavedLogs].stackTrace + "<size=3> </size><br><size=6>-----------------------------[/STACK TRACE]----------------------------</size><br><br>";
                            }
                        }
                    }

                    indexes_ofCurrLog_insideAllLogsList_startingWithNewest[numberOfAlreadyRetrievedMessages] = i_ofAllSavedLogs;
                    numberOfAlreadyRetrievedMessages++;
                }
                if (numberOfAlreadyRetrievedMessages >= maxNumberOfDisplayedLogMessages) { break; }
            }
            return numberOfAlreadyRetrievedMessages;
        }

    }

}
