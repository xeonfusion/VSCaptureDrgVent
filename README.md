# VSCaptureDrgVent
VSCapture module to capture numeric (Settings, Measured Data and Messages) and real time data from Draeger (Medibus.X protocol) ventilators. Requires C# .NET8 (Visual Studio 2022) to compile.

## Note on realtime data
When numeric and realtime data are captured simultaneously, the exact timing of the realtime data may get temporarily offset, while the numeric data is being read. No dataloss should occur. 

A useful workaround is to create a new 'corrected' time column: starttime + row_index * 20 ms. The original time column should be compared, to ensure no drift in time is occurring. 
