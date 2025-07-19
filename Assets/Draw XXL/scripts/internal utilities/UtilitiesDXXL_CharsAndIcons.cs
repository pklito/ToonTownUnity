namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_CharsAndIcons
    {
        ///Symbol definition format:
        //a jagged array of Vector2 arrays,
        //each Vector2 array describes a stringLine of points,
        //multiple stringLine of points describe a symbol.
        //(actually Vector3, but z-component is ignored)

        //saving GC.Alloc and ctor's:
        static Vector3 deletedMarkupStroke_lineStart = new Vector2(0.0f, 0.375f);
        static Vector3 deletedMarkupStroke_lineEnd = new Vector2(1.0f, 0.375f);
        static Vector3 underlinedMarkupStroke_lineStart = new Vector2(0.0f, -0.25f);
        static Vector3 underlinedMarkupStroke_lineEnd = new Vector2(1.0f, -0.25f);

        public static string GetIconAsMarkupString(DrawBasics.IconType requestedIcon)
        {
            switch (requestedIcon)
            {
                case DrawBasics.IconType.profileFoto:
                    return "<icon=profileFoto>";

                case DrawBasics.IconType.imageLandscape:
                    return "<icon=imageLandscape>";

                case DrawBasics.IconType.homeHouse:
                    return "<icon=homeHouse>";

                case DrawBasics.IconType.dataDisc:
                    return "<icon=dataDisc>";

                case DrawBasics.IconType.saveData:
                    return "<icon=saveData>";

                case DrawBasics.IconType.loadData:
                    return "<icon=loadData>";

                case DrawBasics.IconType.speechBubble:
                    return "<icon=speechBubble>";

                case DrawBasics.IconType.speechBubbleEmpty:
                    return "<icon=speechBubbleEmpty>";

                case DrawBasics.IconType.thumbUp:
                    return "<icon=thumbUp>";

                case DrawBasics.IconType.thumbDown:
                    return "<icon=thumbDown>";

                case DrawBasics.IconType.lightBulbOn:
                    return "<icon=lightBulbOn>";

                case DrawBasics.IconType.lightBulbOff:
                    return "<icon=lightBulbOff>";

                case DrawBasics.IconType.videoCamera:
                    return "<icon=videoCamera>";

                case DrawBasics.IconType.camera:
                    return "<icon=camera>";

                case DrawBasics.IconType.music:
                    return "<icon=music>";

                case DrawBasics.IconType.audioSpeaker:
                    return "<icon=audioSpeaker>";

                case DrawBasics.IconType.microphone:
                    return "<icon=microphone>";

                case DrawBasics.IconType.wlan_wifi:
                    return "<icon=wlan_wifi>";

                case DrawBasics.IconType.share:
                    return "<icon=share>";

                case DrawBasics.IconType.timeClock:
                    return "<icon=timeClock>";

                case DrawBasics.IconType.telephone:
                    return "<icon=telephone>";

                case DrawBasics.IconType.doorOpen:
                    return "<icon=doorOpen>";

                case DrawBasics.IconType.doorEnter:
                    return "<icon=doorEnter>";

                case DrawBasics.IconType.doorLeave:
                    return "<icon=doorLeave>";

                case DrawBasics.IconType.locationPin:
                    return "<icon=locationPin>";

                case DrawBasics.IconType.folder:
                    return "<icon=folder>";

                case DrawBasics.IconType.saveToFolder:
                    return "<icon=saveToFolder>";

                case DrawBasics.IconType.loadFromFolder:
                    return "<icon=loadFromFolder>";

                case DrawBasics.IconType.optionsSettingsGear:
                    return "<icon=optionsSettingsGear>";

                case DrawBasics.IconType.adjustOptionsSettings:
                    return "<icon=adjustOptionsSettings>";

                case DrawBasics.IconType.pen:
                    return "<icon=pen>";

                case DrawBasics.IconType.questionMark:
                    return "<icon=questionMark>";

                case DrawBasics.IconType.exclamationMark:
                    return "<icon=exclamationMark>";

                case DrawBasics.IconType.shoppingCart:
                    return "<icon=shoppingCart>";

                case DrawBasics.IconType.checkmarkChecked:
                    return "<icon=checkmarkChecked>";

                case DrawBasics.IconType.checkmarkUnchecked:
                    return "<icon=checkmarkUnchecked>";

                case DrawBasics.IconType.battery:
                    return "<icon=battery>";

                case DrawBasics.IconType.cloud:
                    return "<icon=cloud>";

                case DrawBasics.IconType.magnifier:
                    return "<icon=magnifier>";

                case DrawBasics.IconType.magnifierPlus:
                    return "<icon=magnifierPlus>";

                case DrawBasics.IconType.magnifierMinus:
                    return "<icon=magnifierMinus>";

                case DrawBasics.IconType.timeHourglassCursor:
                    return "<icon=timeHourglassCursor>";

                case DrawBasics.IconType.cursorHand:
                    return "<icon=cursorHand>";

                case DrawBasics.IconType.cursorPointer:
                    return "<icon=cursorPointer>";

                case DrawBasics.IconType.trashcan:
                    return "<icon=trashcan>";

                case DrawBasics.IconType.switchOnOff:
                    return "<icon=switchOnOff>";

                case DrawBasics.IconType.playButton:
                    return "<icon=playButton>";

                case DrawBasics.IconType.pauseButton:
                    return "<icon=pauseButton>";

                case DrawBasics.IconType.stopButton:
                    return "<icon=stopButton>";

                case DrawBasics.IconType.playPauseButton:
                    return "<icon=playPauseButton>";

                case DrawBasics.IconType.heart:
                    return "<icon=heart>";

                case DrawBasics.IconType.coin:
                    return "<icon=coin>";

                case DrawBasics.IconType.coins:
                    return "<icon=coins>";

                case DrawBasics.IconType.moneyBills:
                    return "<icon=moneyBills>";

                case DrawBasics.IconType.moneyBag:
                    return "<icon=moneyBag>";

                case DrawBasics.IconType.chestTreasureBox_closed:
                    return "<icon=chestTreasureBox_closed>";

                case DrawBasics.IconType.lootbox:
                    return "<icon=lootbox>";

                case DrawBasics.IconType.crown:
                    return "<icon=crown>";

                case DrawBasics.IconType.trophy:
                    return "<icon=trophy>";

                case DrawBasics.IconType.awardMedal:
                    return "<icon=awardMedal>";

                case DrawBasics.IconType.sword:
                    return "<icon=sword>";

                case DrawBasics.IconType.shield:
                    return "<icon=shield>";

                case DrawBasics.IconType.gun:
                    return "<icon=gun>";

                case DrawBasics.IconType.bullet:
                    return "<icon=bullet>";

                case DrawBasics.IconType.rocket:
                    return "<icon=rocket>";

                case DrawBasics.IconType.crosshair:
                    return "<icon=crosshair>";

                case DrawBasics.IconType.arrow:
                    return "<icon=arrow>";

                case DrawBasics.IconType.arrowBow:
                    return "<icon=arrowBow>";

                case DrawBasics.IconType.bomb:
                    return "<icon=bomb>";

                case DrawBasics.IconType.shovel:
                    return "<icon=shovel>";

                case DrawBasics.IconType.hammer:
                    return "<icon=hammer>";

                case DrawBasics.IconType.axe:
                    return "<icon=axe>";

                case DrawBasics.IconType.magnet:
                    return "<icon=magnet>";

                case DrawBasics.IconType.compass:
                    return "<icon=compass>";

                case DrawBasics.IconType.fuelStation:
                    return "<icon=fuelStation>";

                case DrawBasics.IconType.fuelCan:
                    return "<icon=fuelCan>";

                case DrawBasics.IconType.lockLocked:
                    return "<icon=lockLocked>";

                case DrawBasics.IconType.lockUnlocked:
                    return "<icon=lockUnlocked>";

                case DrawBasics.IconType.key:
                    return "<icon=key>";

                case DrawBasics.IconType.gemDiamond:
                    return "<icon=gemDiamond>";

                case DrawBasics.IconType.gold:
                    return "<icon=gold>";

                case DrawBasics.IconType.potion:
                    return "<icon=potion>";

                case DrawBasics.IconType.presentGift:
                    return "<icon=presentGift>";

                case DrawBasics.IconType.death:
                    return "<icon=death>";

                case DrawBasics.IconType.map:
                    return "<icon=map>";

                case DrawBasics.IconType.mushroom:
                    return "<icon=mushroom>";

                case DrawBasics.IconType.star:
                    return "<icon=star>";

                case DrawBasics.IconType.pill:
                    return "<icon=pill>";

                case DrawBasics.IconType.health:
                    return "<icon=health>";

                case DrawBasics.IconType.foodPlate:
                    return "<icon=foodPlate>";

                case DrawBasics.IconType.foodMeat:
                    return "<icon=foodMeat>";

                case DrawBasics.IconType.flag:
                    return "<icon=flag>";

                case DrawBasics.IconType.flagChequered:
                    return "<icon=flagChequered>";

                case DrawBasics.IconType.ball:
                    return "<icon=ball>";

                case DrawBasics.IconType.dice:
                    return "<icon=dice>";

                case DrawBasics.IconType.joystick:
                    return "<icon=joystick>";

                case DrawBasics.IconType.gamepad:
                    return "<icon=gamepad>";

                case DrawBasics.IconType.jigsawPuzzle:
                    return "<icon=jigsawPuzzle>";

                case DrawBasics.IconType.fish:
                    return "<icon=fish>";

                case DrawBasics.IconType.car:
                    return "<icon=car>";

                case DrawBasics.IconType.tree:
                    return "<icon=tree>";

                case DrawBasics.IconType.palm:
                    return "<icon=palm>";

                case DrawBasics.IconType.leaf:
                    return "<icon=leaf>";

                case DrawBasics.IconType.nukeNuclearWarning:
                    return "<icon=nukeNuclearWarning>";

                case DrawBasics.IconType.biohazardWarning:
                    return "<icon=biohazardWarning>";

                case DrawBasics.IconType.fireWarning:
                    return "<icon=fireWarning>";

                case DrawBasics.IconType.warning:
                    return "<icon=warning>";

                case DrawBasics.IconType.emergencyExit:
                    return "<icon=emergencyExit>";

                case DrawBasics.IconType.sun:
                    return "<icon=sun>";

                case DrawBasics.IconType.rain:
                    return "<icon=rain>";

                case DrawBasics.IconType.wind:
                    return "<icon=wind>";

                case DrawBasics.IconType.snow:
                    return "<icon=snow>";

                case DrawBasics.IconType.lightning:
                    return "<icon=lightning>";

                case DrawBasics.IconType.fire:
                    return "<icon=fire>";

                case DrawBasics.IconType.unitSquare:
                    return "<icon=unitSquare>";

                case DrawBasics.IconType.unitSquareIncl1Right:
                    return "<icon=unitSquareIncl1Right>";

                case DrawBasics.IconType.unitSquareIncl2Right:
                    return "<icon=unitSquareIncl2Right>";

                case DrawBasics.IconType.unitSquareIncl3Right:
                    return "<icon=unitSquareIncl3Right>";

                case DrawBasics.IconType.unitSquareIncl4Right:
                    return "<icon=unitSquareIncl4Right>";

                case DrawBasics.IconType.unitSquareIncl5Right:
                    return "<icon=unitSquareIncl5Right>";

                case DrawBasics.IconType.unitSquareIncl6Right:
                    return "<icon=unitSquareIncl6Right>";

                case DrawBasics.IconType.unitSquareCrossed:
                    return "<icon=unitSquareCrossed>";

                case DrawBasics.IconType.unitCircle:
                    return "<icon=unitCircle>";

                case DrawBasics.IconType.animal:
                    return "<icon=animal>";

                case DrawBasics.IconType.bird:
                    return "<icon=bird>";

                case DrawBasics.IconType.humanMale:
                    return "<icon=humanMale>";

                case DrawBasics.IconType.humanFemale:
                    return "<icon=humanFemale>";

                case DrawBasics.IconType.bombExplosion:
                    return "<icon=bombExplosion>";

                case DrawBasics.IconType.tower:
                    return "<icon=tower>";

                case DrawBasics.IconType.circleDotFilled:
                    return "<icon=circleDotFilled>";

                case DrawBasics.IconType.circleDotUnfilled:
                    return "<icon=circleDotUnfilled>";

                case DrawBasics.IconType.logMessage:
                    return "<icon=logMessage>";

                case DrawBasics.IconType.logMessageError:
                    return "<icon=logMessageError>";

                case DrawBasics.IconType.logMessageException:
                    return "<icon=logMessageException>";

                case DrawBasics.IconType.logMessageAssertion:
                    return "<icon=logMessageAssertion>";

                case DrawBasics.IconType.up_oneStroke:
                    return "<icon=up_oneStroke>";

                case DrawBasics.IconType.up_twoStroke:
                    return "<icon=up_twoStroke>";

                case DrawBasics.IconType.up_threeStroke:
                    return "<icon=up_threeStroke>";

                case DrawBasics.IconType.down_oneStroke:
                    return "<icon=down_oneStroke>";

                case DrawBasics.IconType.down_twoStroke:
                    return "<icon=down_twoStroke>";

                case DrawBasics.IconType.down_threeStroke:
                    return "<icon=down_threeStroke>";

                case DrawBasics.IconType.left_oneStroke:
                    return "<icon=left_oneStroke>";

                case DrawBasics.IconType.left_twoStroke:
                    return "<icon=left_twoStroke>";

                case DrawBasics.IconType.left_threeStroke:
                    return "<icon=left_threeStroke>";

                case DrawBasics.IconType.right_oneStroke:
                    return "<icon=right_oneStroke>";

                case DrawBasics.IconType.right_twoStroke:
                    return "<icon=right_twoStroke>";

                case DrawBasics.IconType.right_threeStroke:
                    return "<icon=right_threeStroke>";

                case DrawBasics.IconType.fist:
                    return "<icon=fist>";

                case DrawBasics.IconType.boxingGlove:
                    return "<icon=boxingGlove>";

                case DrawBasics.IconType.stars5Rate:
                    return "<icon=stars5Rate>";

                case DrawBasics.IconType.stars3:
                    return "<icon=stars3>";

                case DrawBasics.IconType.shootingStar:
                    return "<icon=shootingStar>";

                case DrawBasics.IconType.moonHalf:
                    return "<icon=moonHalf>";

                case DrawBasics.IconType.moonFullPlanet:
                    return "<icon=moonFullPlanet>";

                case DrawBasics.IconType.leftHandRule:
                    return "<icon=leftHandRule>";

                case DrawBasics.IconType.rightHandRule:
                    return "<icon=rightHandRule>";

                case DrawBasics.IconType.megaphone:
                    return "<icon=megaphone>";

                case DrawBasics.IconType.arrowLeft:
                    return "<icon=arrowLeft>";

                case DrawBasics.IconType.arrowRight:
                    return "<icon=arrowRight>";

                case DrawBasics.IconType.arrowUp:
                    return "<icon=arrowUp>";

                case DrawBasics.IconType.arrowDown:
                    return "<icon=arrowDown>";

                case DrawBasics.IconType.healthBox:
                    return "<icon=healthBox>";

                case DrawBasics.IconType.iceIcicle:
                    return "<icon=iceIcicle>";

                case DrawBasics.IconType.pickAxe:
                    return "<icon=pickAxe>";

                case DrawBasics.IconType.audioSpeakerMute:
                    return "<icon=audioSpeakerMute>";

                case DrawBasics.IconType.chestTreasureBox_open:
                    return "<icon=chestTreasureBox_open>";

                case DrawBasics.IconType.doorClosed:
                    return "<icon=doorClosed>";

                default:
                    Debug.LogError("Icon '" + requestedIcon + "' not implemented.");
                    return "";
            }

        }


        public static void RefillCurrPrintedCharDef(InternalDXXL_CharConfig charToFillIn, out bool charIsMissing)
        {
            Vector3[][] charDefinition;
            if (charToFillIn.isIcon)
            {
                charDefinition = DrawXXL_LinesManager.instance.GetPointsArray(charToFillIn.iconString, out charIsMissing);
            }
            else
            {
                charDefinition = DrawXXL_LinesManager.instance.GetPointsArray(charToFillIn.character, out charIsMissing);
            }

            int addionalMarkupStrokes = 0;
            if (charToFillIn.deleted)
            {
                addionalMarkupStrokes++;
            }
            if (charToFillIn.underlined)
            {
                addionalMarkupStrokes++;
            }

            DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar = charDefinition.Length + addionalMarkupStrokes;
            for (int i_stroke = 0; i_stroke < charDefinition.Length; i_stroke++)
            {
                DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[i_stroke] = charDefinition[i_stroke].Length;
                for (int i_point = 0; i_point < charDefinition[i_stroke].Length; i_point++)
                {
                    DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point] = charDefinition[i_stroke][i_point];
                }
            }

            int curr_stroke = charDefinition.Length;
            if (charToFillIn.deleted)
            {
                DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[curr_stroke] = 2;
                DrawXXL_LinesManager.instance.currPrinted_charDef[curr_stroke][0] = deletedMarkupStroke_lineStart;
                DrawXXL_LinesManager.instance.currPrinted_charDef[curr_stroke][1] = deletedMarkupStroke_lineEnd;
                curr_stroke++;
            }
            if (charToFillIn.underlined)
            {
                DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[curr_stroke] = 2;
                DrawXXL_LinesManager.instance.currPrinted_charDef[curr_stroke][0] = underlinedMarkupStroke_lineStart;
                DrawXXL_LinesManager.instance.currPrinted_charDef[curr_stroke][1] = underlinedMarkupStroke_lineEnd;
                curr_stroke++;
            }
        }


        static Vector3 offset_toShiftIconsCenterToZero = new Vector3(-0.5f, -0.5f, 0.0f);
        public static void RefillCurrPrintedCharDefWithZeroCenteredIcon(DrawBasics.IconType iconToFillIn)
        {
            Vector3[][] charDefinition = DrawXXL_LinesManager.instance.GetPointsArray(iconToFillIn);
            DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar = charDefinition.Length;
            for (int i_stroke = 0; i_stroke < charDefinition.Length; i_stroke++)
            {
                DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[i_stroke] = charDefinition[i_stroke].Length;
                for (int i_point = 0; i_point < charDefinition[i_stroke].Length; i_point++)
                {
                    DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point] = charDefinition[i_stroke][i_point] + offset_toShiftIconsCenterToZero;
                }
            }
        }


        public static void DrawAllIconsWithTheirNames(Vector3 position, Color iconsColor, Color textColor, bool displayNameTexts, float sizeOfIconWall)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            iconsColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(iconsColor, Color.white);
            textColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(textColor, Color.black);
            sizeOfIconWall = Mathf.Max(sizeOfIconWall, 1.0f);
            Vector3 positionOfNextThemeBlock = position;

            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "System / Operate", 0, 37, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Human", 38, 45, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Nature / Weather", 46, 63, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Games", 64, 113, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Tools / Weapons", 114, 124, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Signs / Warning", 125, 133, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Basics", 134, 164, iconsColor, textColor, sizeOfIconWall, displayNameTexts, 158);
            positionOfNextThemeBlock = DrawIconThemeBlock(positionOfNextThemeBlock, "Miscellaneous", 165, 166, iconsColor, textColor, sizeOfIconWall, displayNameTexts, (-1));
        }

        static Vector3 DrawIconThemeBlock(Vector3 startPos, string headline, int i_startIcon, int i_endIcon, Color iconsColor, Color textColor, float sizeOfWholeIconWall, bool displayNameTexts, int newLineForEveryIconAfterThisIconI)
        {
            Vector3 currPosOfLineStart = startPos;
            float iconSize = 0.1f * sizeOfWholeIconWall;
            float lineHeight = 1.8f * iconSize;
            UtilitiesDXXL_Text.Write(headline + ":", currPosOfLineStart + Vector3.left * 0.5f * iconSize, iconsColor, 0.65f * iconSize, Vector3.right, Vector3.up, DrawText.TextAnchorDXXL.MiddleLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, false, false, true);
            currPosOfLineStart = currPosOfLineStart + Vector3.down * lineHeight;
            int i_line = 0;
            int i_ofIconInsideLine = 0;
            int iconsPerLine = 10;
            bool hasAlreadyDrawnAnIconInCurrLine = false;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            for (int i_icon = i_startIcon; i_icon <= i_endIcon; i_icon++)
            {
                Vector3 currIconPosition = currPosOfLineStart + Vector3.right * 1.8f * iconSize * i_ofIconInsideLine;
                string text = ((DrawBasics.IconType)i_icon).ToString();
                UtilitiesDXXL_DrawBasics.Icon(currIconPosition, (DrawBasics.IconType)i_icon, iconsColor, iconSize, displayNameTexts ? DrawText.MarkupColor(text, textColor) : null, rotation, 0, false, 0.0f, false, 0.1f, 0.004f, true);
                hasAlreadyDrawnAnIconInCurrLine = true;
                i_ofIconInsideLine++;

                if (i_icon == newLineForEveryIconAfterThisIconI)
                {
                    iconsPerLine = 1;
                }
                if (i_ofIconInsideLine >= iconsPerLine)
                {
                    i_ofIconInsideLine = 0;
                    i_line++;
                    currPosOfLineStart = currPosOfLineStart + Vector3.down * lineHeight;
                    hasAlreadyDrawnAnIconInCurrLine = false;
                }
            }

            Vector3 positionOfNextThemeBlock = currPosOfLineStart + 0.65f * Vector3.down * lineHeight;
            if (hasAlreadyDrawnAnIconInCurrLine)
            {
                positionOfNextThemeBlock = positionOfNextThemeBlock + Vector3.down * lineHeight;
            }
            return positionOfNextThemeBlock;
        }

    }

}
