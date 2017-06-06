# SES - Session End Simulator
A small utility to signal a Windows application that the current user session is ending. For debugging restart problems in applications.

Microsoft used to make a tool like this available back in Vista days, however, I was unable to locate it (download link is dead). There are several gigs of ISO's listed now for this "tool", that don't show the specific required executable in any sort of manifest. This project was created to replace this now dead/missing/buried utility.

This program will grab all window handles for the target process (by PID), send all the same messages that Windows does to apps during shutdown, then wait 5 seconds for the app to gracefully shut down, just as Windows does before force killing it. After the 5 seconds have elapsed, this program WILL NOT force terminate the target process, but rather will let you know if the target process gracefully exited as a proper response to the window messages that were pushed to it.

This is useful for accurately debugging applications that are coded to respond to the Windows shutdown/logoff/etc (session end) event(s).

### How To Use It

Get the PID of the target process (a process that has a UI) from tha Task Manager, under the Details tab. Then, run the command:

`ses.exe TARGET_PID`

And that's it. Note however, that if you're trying to run this utility on a process with elevated permissions (running as Admin), you'll need to also run this utility from a console that has Admin privileges. Basic security logic would dictate that a non-admin program shouldn't be able to simply pump messages into an Admin program that would cause it to shut down, and fortunately Microsoft got this security logic right. So, make sure you're running this program with the same privileges as the target.
