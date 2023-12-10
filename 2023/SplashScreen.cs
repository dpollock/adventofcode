
using System;

namespace AdventOfCode.Y2023
{
   class SplashScreenImpl : ISplashScreen {

      public void Show() {

         var color = Console.ForegroundColor;
            Write(0xcc00, false, "           ▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄\n           █▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄ ");
        Write(0xcc00, false, " █  █ █ █ █ █▄█\n           █ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  λy.2023\n            \n    ");
        Write(0xcc00, false, "                                                        \n                                           ");
        Write(0xcc00, false, "                 \n                                                            \n                     ");
        Write(0xcc00, false, "                                       \n                                                            ");
        Write(0xcc00, false, "\n                                                            \n           ");
        Write(0x333333, false, "'                       '                        \n                 *                                ");
        Write(0x333333, false, "            ");
        Write(0x666666, false, "11\n               '.      *     ..'");
        Write(0xe3b585, false, "' ''...                       ");
        Write(0xcccccc, false, "10 ");
        Write(0xffff66, false, "*");
        Write(0x666666, false, "*\n                     ");
        Write(0xe3b585, false, ".");
        Write(0x333333, false, "'''");
        Write(0xe3b585, false, "~ ~ ~ ~   ");
        Write(0x6b4d3b, false, "### ");
        Write(0xe3b585, false, "''.                  \n                   .' ~  ");
        Write(0xcc00, false, ",");
        Write(0xffff66, true, "* ");
        Write(0xe3b585, false, "~ ~ ~ ~ ");
        Write(0x6b4d3b, false, "##### ");
        Write(0xe3b585, false, "'.                  ");
        Write(0xcccccc, false, " 9 ");
        Write(0xffff66, false, "**\n                   ");
        Write(0xe3b585, false, ": ~ ");
        Write(0xcc00, false, "'");
        Write(0x5555bb, false, "(~)");
        Write(0xcc00, false, ", ");
        Write(0xe3b585, false, "~ ");
        Write(0xffff66, true, "* ");
        Write(0xe3b585, false, "~ ~ ~ ");
        Write(0x6b4d3b, false, "### ");
        Write(0xe3b585, false, ":                  ");
        Write(0xcccccc, false, " 8 ");
        Write(0xffff66, false, "**\n                   ");
        Write(0xe3b585, false, "'. ~ ");
        Write(0xcc00, false, "\" ' ");
        Write(0xe3b585, false, "~ ~ ~   ");
        Write(0x6b4d3b, false, "##### ");
        Write(0xe3b585, false, ".'                \n                     '.. ~ ~ ");
        Write(0xffff66, true, "* ");
        Write(0xe3b585, false, "~ ");
        Write(0x6b4d3b, false, "##### ");
        Write(0xe3b585, false, "..'");
        Write(0xcc00, false, ".'''''''''...       ");
        Write(0xcccccc, false, " 7 ");
        Write(0xffff66, false, "**\n                        ");
        Write(0xe3b585, false, "'''.........'''");
        Write(0xcc00, false, "' ");
        Write(0x5555bb, false, "~ ");
        Write(0xcc00, false, ".'");
        Write(0xffff66, true, "*");
        Write(0xcc00, false, ". ");
        Write(0x5555bb, false, "~  ");
        Write(0xcc00, false, "..  ''.    ");
        Write(0xcccccc, false, " 6 ");
        Write(0xffff66, false, "**\n                                   ");
        Write(0xcc00, false, ".' ");
        Write(0x5555bb, false, "~    ");
        Write(0xcc00, false, "'...' ");
        Write(0x5555bb, false, "~");
        Write(0xcc00, false, "'  '.");
        Write(0x5555bb, false, "~  ");
        Write(0xcc00, false, "'.\n                                   :         ");
        Write(0x5555bb, false, "~     ");
        Write(0xcc00, false, "'. ");
        Write(0xffff66, true, "*");
        Write(0xcc00, false, "'.");
        Write(0x5555bb, false, "~ ");
        Write(0xcc00, false, ":  ");
        Write(0xcccccc, false, " 5 ");
        Write(0xffff66, false, "**\n                            ");
        Write(0xffffff, false, "...''''");
        Write(0xcc00, false, "'.         .''.");
        Write(0x5555bb, false, "~  ");
        Write(0xcc00, false, "'..' .'\n                         ");
        Write(0xffffff, false, ".''         ");
        Write(0xcc00, false, "'..  ");
        Write(0x5555bb, false, "~");
        Write(0xcc00, false, "..'");
        Write(0xffff66, true, "*   ");
        Write(0xcc00, false, "'. ");
        Write(0x5555bb, false, "~ ");
        Write(0xcc00, false, "..'    ");
        Write(0xcccccc, false, " 4 ");
        Write(0xffff66, false, "**\n                       ");
        Write(0xffffff, false, ".'               ");
        Write(0xcc00, false, "'''..");
        Write(0xd4dde4, false, "/");
        Write(0xcc00, false, "......'''     \n                       ");
        Write(0xffffff, false, ":             /\\    ");
        Write(0xccccff, false, "-");
        Write(0xd4dde4, false, "/  ");
        Write(0xffffff, false, ":            \n                       '.            ");
        Write(0xccccff, false, "-   - ");
        Write(0xd4dde4, false, "/  ");
        Write(0xffffff, false, ".'            \n                         '..    ");
        Write(0xccccff, false, "-     -   ");
        Write(0xffff66, true, "*");
        Write(0xffffff, false, "..'                ");
        Write(0xcccccc, false, " 3 ");
        Write(0xffff66, false, "**\n               ");
        Write(0x9b715b, false, "----@        ");
        Write(0xffffff, false, "'''..");
        Write(0xffff66, true, "*");
        Write(0xffffff, false, "......'''                   ");
        Write(0xcccccc, false, " 2 ");
        Write(0xffff66, false, "**\n             ");
        Write(0xffff66, true, "* ");
        Write(0x9b715b, false, "! /^\\                                          ");
        Write(0xcccccc, false, " 1 ");
        Write(0xffff66, false, "**\n           \n");
        
            Console.ForegroundColor = color;
           Console.WriteLine();
        }

       private static void Write(int rgb, bool bold, string text){
           Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
       }
   }
}
