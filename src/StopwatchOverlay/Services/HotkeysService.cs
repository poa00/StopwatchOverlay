﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StopwatchOverlay.MainWindow;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Interop;

namespace StopwatchOverlay.Logic
{
    public class HotkeysService
    {
        private TimekeepingService timekeeper;
        private Window appWindow;

        private int windowPosIndex;

        public HotkeysService(TimekeepingService timekeeper, Window appWindow)
        {
            this.timekeeper = timekeeper;
            this.appWindow = appWindow;

            var ShowControlsHotkey = new DetectHotkeyService(Key.F1, KeyModifier.Shift | KeyModifier.Win, ShowControls);

            var StartStopHotkey = new DetectHotkeyService(Key.F7, KeyModifier.Shift | KeyModifier.Win, StartStop);

            var ResetHotkey = new DetectHotkeyService(Key.F8, KeyModifier.Shift | KeyModifier.Win, Reset);

            var SwitchCornerHotkey = new DetectHotkeyService(Key.F9, KeyModifier.Shift | KeyModifier.Win, SwitchCorner);
            this.windowPosIndex = 1;

            var ExitApplicationHotkey = new DetectHotkeyService(Key.F10, KeyModifier.Shift | KeyModifier.Win, ExitApplication);
        }

        private void StartStop(DetectHotkeyService hotKey)
        {
            if (this.timekeeper.IsRunning())
            {
                this.timekeeper.Stop();
                return;
            }

            this.timekeeper.Start();
        }

        private void Reset(DetectHotkeyService hotKey)
        {
            this.timekeeper.Reset();
        }

        private void SwitchCorner(DetectHotkeyService hotkey)
        {
            var screenWidth = System.Windows.SystemParameters.WorkArea.Width;
            var screenHeight = System.Windows.SystemParameters.WorkArea.Height;

            Point topRightPoint = new Point(screenWidth - this.appWindow.Width - 25, 25);
            Point bottomRightPoint = new Point(screenWidth - this.appWindow.Width - 25, screenHeight - this.appWindow.Height - 25);
            Point bottomLeftPoint = new Point(25, screenHeight - this.appWindow.Height - 25);
            Point topLeftPoint = new Point(25, 25);

            this.windowPosIndex++;

            if (this.windowPosIndex == 1)
            {
                this.appWindow.Left = topRightPoint.X;
                this.appWindow.Top = topRightPoint.Y;
            }
            else if (this.windowPosIndex == 2)
            {
                this.appWindow.Left = bottomRightPoint.X;
                this.appWindow.Top = bottomRightPoint.Y;
            }
            else if (this.windowPosIndex == 3)
            {
                this.appWindow.Left = bottomLeftPoint.X;
                this.appWindow.Top = bottomLeftPoint.Y;
            }
            else if (this.windowPosIndex == 4)
            {
                this.appWindow.Left = topLeftPoint.X;
                this.appWindow.Top = topLeftPoint.Y;
            }
            else
            {
                this.windowPosIndex = 1;

                this.appWindow.Left = topRightPoint.X;
                this.appWindow.Top = topRightPoint.Y;
            }
        }

        private void ExitApplication(DetectHotkeyService hotkey)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ShowControls(DetectHotkeyService hotkey)
        {
            if (!Application.Current.Windows.Cast<Window>().Any(x => x.Title == "CONTROLS"))
            {
                MessageBox.Show(
                    "CONTROLS",
                    "SHIFT + WIN + F7  - Start/Stop\n\n" +
                    "SHIFT + WIN + F8  - Restart\n\n" +
                    "SHIFT + WIN + F9  - Change Position\n\n" +
                    "SHIFT + WIN + F10 - Exit Application");
            }
        }
    }
}
