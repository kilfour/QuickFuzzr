## unknown
### FlatBenchmarks
| Method       | Mean     | Error    | StdDev   | Gen0      | Gen1     | Gen2     | Allocated |
|------------- |---------:|---------:|---------:|----------:|---------:|---------:|----------:|
| AutoFuzzr    | 49.99 ms | 0.864 ms | 1.293 ms | 5200.0000 | 800.0000 | 400.0000 |  42.52 MB |
| ConfigFuzzr  | 43.87 ms | 0.874 ms | 1.387 ms | 7500.0000 | 750.0000 | 250.0000 |  61.38 MB |
| FactoryFuzzr | 24.04 ms | 0.428 ms | 0.380 ms | 5593.7500 | 875.0000 | 406.2500 |  44.82 MB |
## unknown
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 48.75 ms | 0.598 ms | 0.559 ms | 5200.0000 |  800.0000 | 400.0000 |  42.54 MB |
| ConfigFuzzr          | 48.87 ms | 0.924 ms | 0.949 ms | 8750.0000 | 1250.0000 | 750.0000 |  71.83 MB |
| PreloadedConfigFuzzr | 13.90 ms | 0.135 ms | 0.120 ms | 1906.2500 |  671.8750 | 406.2500 |   15.3 MB |
| FactoryFuzzr         | 23.68 ms | 0.365 ms | 0.324 ms | 5062.5000 |  937.5000 | 562.5000 |  40.62 MB |
## reverted prop setvalue cache
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| AutoFuzzr            | 49.43 ms | 0.615 ms | 0.545 ms | 5000.0000 |  800.0000 |  400.0000 |  41.39 MB |
| ConfigFuzzr          | 49.26 ms | 0.967 ms | 1.035 ms | 8750.0000 | 2000.0000 | 1000.0000 |  70.68 MB |
| PreloadedConfigFuzzr | 11.58 ms | 0.151 ms | 0.142 ms | 1765.6250 |  781.2500 |  328.1250 |  14.15 MB |
| FactoryFuzzr         | 24.06 ms | 0.457 ms | 0.470 ms | 5062.5000 |  937.5000 |  593.7500 |  40.62 MB |
## added nullability cache
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| AutoFuzzr            | 23.99 ms | 0.174 ms | 0.162 ms | 3031.2500 |  656.2500 |  281.2500 |  24.21 MB |
| ConfigFuzzr          | 51.50 ms | 0.687 ms | 0.643 ms | 8750.0000 | 2750.0000 | 1500.0000 |  71.76 MB |
| PreloadedConfigFuzzr | 11.62 ms | 0.219 ms | 0.205 ms | 1765.6250 |  796.8750 |  343.7500 |  14.16 MB |
| FactoryFuzzr         | 23.75 ms | 0.368 ms | 0.344 ms | 5062.5000 |  937.5000 |  593.7500 |  40.62 MB |
## after code review
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 20.39 ms | 0.230 ms | 0.204 ms | 2968.7500 |  625.0000 | 312.5000 |  23.69 MB |
| ConfigFuzzr          | 48.45 ms | 0.728 ms | 0.681 ms | 9000.0000 | 1250.0000 | 750.0000 |  72.06 MB |
| PreloadedConfigFuzzr | 10.99 ms | 0.140 ms | 0.131 ms | 1765.6250 |  812.5000 | 359.3750 |  14.16 MB |
| FactoryFuzzr         | 21.94 ms | 0.429 ms | 0.440 ms | 5062.5000 |  937.5000 | 562.5000 |  40.62 MB |
### PseudopolisBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| AutoFuzzr            | 22.07 ms | 0.167 ms | 0.140 ms | 2968.7500 |  656.2500 |  375.0000 |  23.68 MB |
| ConfigFuzzr          | 48.29 ms | 0.838 ms | 0.784 ms | 8750.0000 | 2000.0000 | 1000.0000 |  70.69 MB |
| PreloadedConfigFuzzr | 11.07 ms | 0.069 ms | 0.054 ms | 1765.6250 |  796.8750 |  343.7500 |  14.16 MB |
### Trees
| Method               | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|--------------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| ConfigFuzzr          | 4.125 ms | 0.0592 ms | 0.0554 ms | 828.1250 | 304.6875 |   6.61 MB |
| PreloadedConfigFuzzr | 2.714 ms | 0.0174 ms | 0.0154 ms | 597.6563 | 195.3125 |   4.78 MB |

## Simplified StuffToApply lookup in Genesis.BuildInstance
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| AutoFuzzr            | 21.99 ms | 0.074 ms | 0.066 ms | 2968.7500 |  656.2500 |  375.0000 |  23.68 MB |
| ConfigFuzzr          | 49.23 ms | 0.848 ms | 0.793 ms | 8750.0000 | 2750.0000 | 1500.0000 |  71.45 MB |
| PreloadedConfigFuzzr | 11.50 ms | 0.122 ms | 0.108 ms | 1765.6250 |  796.8750 |  343.7500 |  14.15 MB |
| FactoryFuzzr         | 23.28 ms | 0.405 ms | 0.359 ms | 5062.5000 |  937.5000 |  562.5000 |  40.62 MB |
### PseudopolisBenchmarks
 Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| AutoFuzzr            | 23.12 ms | 0.260 ms | 0.231 ms | 2968.7500 |  656.2500 |  375.0000 |  23.68 MB |
| ConfigFuzzr          | 48.02 ms | 0.782 ms | 0.653 ms | 8750.0000 | 2000.0000 | 1000.0000 |  70.68 MB |
| PreloadedConfigFuzzr | 11.89 ms | 0.148 ms | 0.132 ms | 1765.6250 |  796.8750 |  359.3750 |  14.16 MB |
### Trees
| Method               | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|--------------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| ConfigFuzzr          | 4.101 ms | 0.0701 ms | 0.0656 ms | 828.1250 | 304.6875 |   6.61 MB |
| PreloadedConfigFuzzr | 2.995 ms | 0.0422 ms | 0.0374 ms | 593.7500 | 187.5000 |   4.78 MB |

## IsPrimitive lookup simplified + change inheritence handling 
### FlatBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 21.52 ms | 0.248 ms | 0.232 ms | 2937.5000 |  625.0000 | 281.2500 |  23.46 MB |
| ConfigFuzzr          | 47.30 ms | 0.871 ms | 0.815 ms | 8750.0000 | 1250.0000 | 750.0000 |  70.45 MB |
| PreloadedConfigFuzzr | 10.48 ms | 0.161 ms | 0.143 ms | 1734.3750 |  750.0000 | 265.6250 |  13.93 MB |
| FactoryFuzzr         | 22.76 ms | 0.414 ms | 0.367 ms | 5062.5000 |  937.5000 | 593.7500 |   40.4 MB |
### HorsesForCoursesBenchmarks
| Method       | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| FromTheGuide | 3.634 ms | 0.0700 ms | 0.0688 ms | 898.4375 | 500.0000 |    7.2 MB |
### PseudopolisBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 21.82 ms | 0.201 ms | 0.179 ms | 2937.5000 |  625.0000 | 312.5000 |  23.47 MB |
| ConfigFuzzr          | 48.68 ms | 0.947 ms | 1.128 ms | 8750.0000 | 1250.0000 | 750.0000 |  70.47 MB |
| PreloadedConfigFuzzr | 11.25 ms | 0.141 ms | 0.117 ms | 1734.3750 |  750.0000 | 234.3750 |  13.94 MB |
### Trees
| Method               | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|--------------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| ConfigFuzzr          | 4.095 ms | 0.0575 ms | 0.0538 ms | 789.0625 | 289.0625 |   6.33 MB |
| PreloadedConfigFuzzr | 2.990 ms | 0.0305 ms | 0.0285 ms | 562.5000 | 210.9375 |    4.5 MB |

## Cache Property IsInitOnly
### FlatBenchmarks
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 16.09 ms | 0.264 ms | 0.220 ms | 2781.2500 |  750.0000 | 375.0000 |  22.31 MB |
| ConfigFuzzr          | 47.16 ms | 0.765 ms | 0.715 ms | 8750.0000 | 1250.0000 | 750.0000 |  70.46 MB |
| PreloadedConfigFuzzr | 11.16 ms | 0.142 ms | 0.126 ms | 1734.3750 |  750.0000 | 265.6250 |  13.93 MB |
| FactoryFuzzr         | 18.27 ms | 0.214 ms | 0.190 ms | 4906.2500 |  875.0000 | 750.0000 |  39.25 MB |
### HorsesForCoursesBenchmarks
| Method       | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| FromTheGuide | 3.686 ms | 0.0577 ms | 0.0512 ms | 890.6250 | 421.8750 |   7.16 MB |
### PseudopolisBenchmarks
| Method               | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|--------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoFuzzr            | 16.20 ms | 0.318 ms | 0.367 ms | 2781.2500 |  750.0000 | 375.0000 |  22.32 MB |
| ConfigFuzzr          | 50.76 ms | 1.001 ms | 1.436 ms | 8750.0000 | 1250.0000 | 750.0000 |  70.47 MB |
| PreloadedConfigFuzzr | 11.59 ms | 0.149 ms | 0.132 ms | 1734.3750 |  750.0000 | 234.3750 |  13.94 MB |
### Trees
| Method               | Mean     | Error     | StdDev    | Gen0     | Gen1     | Allocated |
|--------------------- |---------:|----------:|----------:|---------:|---------:|----------:|
| ConfigFuzzr          | 3.737 ms | 0.0565 ms | 0.0529 ms | 781.2500 | 277.3438 |   6.24 MB |
| PreloadedConfigFuzzr | 2.512 ms | 0.0492 ms | 0.0483 ms | 550.7813 | 210.9375 |   4.42 MB |
## 
### FlatBenchmarks
### HorsesForCoursesBenchmarks
### PseudopolisBenchmarks
### Trees