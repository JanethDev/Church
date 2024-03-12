; ModuleID = 'obj\Debug\110\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Debug\110\android\marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [280 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 93
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 123
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 12
	i32 39852199, ; 3: Plugin.Permissions => 0x26018a7 => 17
	i32 57263871, ; 4: Xamarin.Forms.Core.dll => 0x369c6ff => 117
	i32 57967248, ; 5: Xamarin.Android.Support.VersionedParcelable.dll => 0x3748290 => 66
	i32 99966887, ; 6: Xamarin.Firebase.Iid.dll => 0x5f55fa7 => 113
	i32 101534019, ; 7: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 103
	i32 120558881, ; 8: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 103
	i32 148395041, ; 9: Lottie.Forms.dll => 0x8d85421 => 9
	i32 160529393, ; 10: Xamarin.Android.Arch.Core.Common => 0x9917bf1 => 36
	i32 165246403, ; 11: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 78
	i32 166922606, ; 12: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 47
	i32 182336117, ; 13: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 104
	i32 201930040, ; 14: Xamarin.Android.Arch.Core.Runtime => 0xc093538 => 37
	i32 209399409, ; 15: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 76
	i32 219130465, ; 16: Xamarin.Android.Support.v4 => 0xd0faa61 => 63
	i32 230216969, ; 17: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 88
	i32 232815796, ; 18: System.Web.Services => 0xde07cb4 => 135
	i32 278686392, ; 19: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 92
	i32 280482487, ; 20: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 86
	i32 318968648, ; 21: Xamarin.AndroidX.Activity.dll => 0x13031348 => 68
	i32 321597661, ; 22: System.Numerics => 0x132b30dd => 30
	i32 329550603, ; 23: Plugin.LocalNotifications => 0x13a48b0b => 16
	i32 342366114, ; 24: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 90
	i32 347068432, ; 25: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 23
	i32 385762202, ; 26: System.Memory.dll => 0x16fe439a => 28
	i32 388313361, ; 27: Xamarin.Android.Support.Animated.Vector.Drawable => 0x17253111 => 43
	i32 389971796, ; 28: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 49
	i32 442521989, ; 29: Xamarin.Essentials => 0x1a605985 => 111
	i32 450948140, ; 30: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 85
	i32 465846621, ; 31: mscorlib => 0x1bc4415d => 11
	i32 469710990, ; 32: System.dll => 0x1bff388e => 27
	i32 476646585, ; 33: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 86
	i32 486930444, ; 34: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 97
	i32 514659665, ; 35: Xamarin.Android.Support.Compat => 0x1ead1551 => 47
	i32 524864063, ; 36: Xamarin.Android.Support.Print => 0x1f48ca3f => 60
	i32 526420162, ; 37: System.Transactions.dll => 0x1f6088c2 => 129
	i32 542030372, ; 38: Xamarin.GooglePlayServices.Stats => 0x204eba24 => 126
	i32 545304856, ; 39: System.Runtime.Extensions => 0x2080b118 => 137
	i32 605376203, ; 40: System.IO.Compression.FileSystem => 0x24154ecb => 133
	i32 627609679, ; 41: Xamarin.AndroidX.CustomView => 0x2568904f => 82
	i32 663517072, ; 42: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 108
	i32 666292255, ; 43: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 73
	i32 690569205, ; 44: System.Xml.Linq.dll => 0x29293ff5 => 35
	i32 692692150, ; 45: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 44
	i32 748832960, ; 46: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 21
	i32 775507847, ; 47: System.IO.Compression => 0x2e394f87 => 132
	i32 782533833, ; 48: Xamarin.Google.AutoValue.Annotations.dll => 0x2ea484c9 => 122
	i32 801787702, ; 49: Xamarin.Android.Support.Interpolator => 0x2fca4f36 => 56
	i32 809851609, ; 50: System.Drawing.Common.dll => 0x30455ad9 => 131
	i32 843511501, ; 51: Xamarin.AndroidX.Print => 0x3246f6cd => 100
	i32 882883187, ; 52: Xamarin.Android.Support.v4.dll => 0x349fba73 => 63
	i32 902159924, ; 53: Rg.Plugins.Popup => 0x35c5de34 => 19
	i32 916714535, ; 54: Xamarin.Android.Support.Print.dll => 0x36a3f427 => 60
	i32 928116545, ; 55: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 123
	i32 955402788, ; 56: Newtonsoft.Json => 0x38f24a24 => 12
	i32 957807352, ; 57: Plugin.CurrentActivity => 0x3916faf8 => 13
	i32 958213972, ; 58: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 59
	i32 967690846, ; 59: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 90
	i32 974778368, ; 60: FormsViewGroup.dll => 0x3a19f000 => 6
	i32 987342438, ; 61: Xamarin.Android.Arch.Lifecycle.LiveData.dll => 0x3ad9a666 => 40
	i32 1012816738, ; 62: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 102
	i32 1035644815, ; 63: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 72
	i32 1042160112, ; 64: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 119
	i32 1052210849, ; 65: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 94
	i32 1061503568, ; 66: Xamarin.Google.AutoValue.Annotations => 0x3f454250 => 122
	i32 1098167829, ; 67: Xamarin.Android.Arch.Lifecycle.ViewModel => 0x4174b615 => 42
	i32 1098259244, ; 68: System => 0x41761b2c => 27
	i32 1137654822, ; 69: Plugin.Permissions.dll => 0x43cf3c26 => 17
	i32 1141947663, ; 70: Xamarin.Firebase.Measurement.Connector.dll => 0x4410bd0f => 115
	i32 1175144683, ; 71: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 106
	i32 1192522218, ; 72: CardView => 0x471471ea => 4
	i32 1204270330, ; 73: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 73
	i32 1267360935, ; 74: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 107
	i32 1292207520, ; 75: SQLitePCLRaw.core.dll => 0x4d0585a0 => 22
	i32 1292763917, ; 76: Xamarin.Android.Support.CursorAdapter.dll => 0x4d0e030d => 51
	i32 1293217323, ; 77: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 84
	i32 1297413738, ; 78: Xamarin.Android.Arch.Lifecycle.LiveData.Core => 0x4d54f66a => 39
	i32 1309284514, ; 79: Plugin.FirebasePushNotification => 0x4e0a18a2 => 14
	i32 1333047053, ; 80: Xamarin.Firebase.Common => 0x4f74af0d => 112
	i32 1365406463, ; 81: System.ServiceModel.Internals.dll => 0x516272ff => 136
	i32 1376866003, ; 82: Xamarin.AndroidX.SavedState => 0x52114ed3 => 102
	i32 1379779777, ; 83: System.Resources.ResourceManager => 0x523dc4c1 => 3
	i32 1395857551, ; 84: Xamarin.AndroidX.Media.dll => 0x5333188f => 98
	i32 1406073936, ; 85: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 79
	i32 1411638395, ; 86: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 32
	i32 1445445088, ; 87: Xamarin.Android.Support.Fragment => 0x5627bde0 => 55
	i32 1457743152, ; 88: System.Runtime.Extensions.dll => 0x56e36530 => 137
	i32 1460219004, ; 89: Xamarin.Forms.Xaml => 0x57092c7c => 120
	i32 1462112819, ; 90: System.IO.Compression.dll => 0x57261233 => 132
	i32 1469204771, ; 91: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 71
	i32 1531040989, ; 92: Xamarin.Firebase.Iid.Interop.dll => 0x5b41d4dd => 114
	i32 1574652163, ; 93: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 50
	i32 1582372066, ; 94: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 83
	i32 1587447679, ; 95: Xamarin.Android.Arch.Core.Common.dll => 0x5e9e877f => 36
	i32 1592978981, ; 96: System.Runtime.Serialization.dll => 0x5ef2ee25 => 2
	i32 1622152042, ; 97: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 96
	i32 1624863272, ; 98: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 110
	i32 1636350590, ; 99: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 81
	i32 1639515021, ; 100: System.Net.Http.dll => 0x61b9038d => 29
	i32 1657153582, ; 101: System.Runtime => 0x62c6282e => 33
	i32 1658251792, ; 102: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 121
	i32 1662014763, ; 103: Xamarin.Android.Support.Vector.Drawable => 0x6310552b => 65
	i32 1711441057, ; 104: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 23
	i32 1729485958, ; 105: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 77
	i32 1766324549, ; 106: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 104
	i32 1776026572, ; 107: System.Core.dll => 0x69dc03cc => 26
	i32 1788241197, ; 108: Xamarin.AndroidX.Fragment => 0x6a96652d => 85
	i32 1808609942, ; 109: Xamarin.AndroidX.Loader => 0x6bcd3296 => 96
	i32 1813201214, ; 110: Xamarin.Google.Android.Material => 0x6c13413e => 121
	i32 1818375724, ; 111: Plugin.LocalNotifications.Abstractions => 0x6c62362c => 15
	i32 1866717392, ; 112: Xamarin.Android.Support.Interpolator.dll => 0x6f43d8d0 => 56
	i32 1867746548, ; 113: Xamarin.Essentials.dll => 0x6f538cf4 => 111
	i32 1878053835, ; 114: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 120
	i32 1885316902, ; 115: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 74
	i32 1891780326, ; 116: Church.Mobile.Android => 0x70c246e6 => 0
	i32 1900610850, ; 117: System.Resources.ResourceManager.dll => 0x71490522 => 3
	i32 1908813208, ; 118: Xamarin.GooglePlayServices.Basement => 0x71c62d98 => 125
	i32 1916660109, ; 119: Xamarin.Android.Arch.Lifecycle.ViewModel.dll => 0x723de98d => 42
	i32 1919157823, ; 120: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 99
	i32 1933215285, ; 121: Xamarin.Firebase.Messaging.dll => 0x733a8635 => 116
	i32 2011961780, ; 122: System.Buffers.dll => 0x77ec19b4 => 25
	i32 2019465201, ; 123: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 94
	i32 2037417872, ; 124: Xamarin.Android.Support.ViewPager => 0x79708790 => 67
	i32 2044222327, ; 125: Xamarin.Android.Support.Loader => 0x79d85b77 => 57
	i32 2055257422, ; 126: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 91
	i32 2079903147, ; 127: System.Runtime.dll => 0x7bf8cdab => 33
	i32 2090596640, ; 128: System.Numerics.Vectors => 0x7c9bf920 => 31
	i32 2097448633, ; 129: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 87
	i32 2103459038, ; 130: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 24
	i32 2126786730, ; 131: Xamarin.Forms.Platform.Android => 0x7ec430aa => 118
	i32 2129483829, ; 132: Xamarin.GooglePlayServices.Base.dll => 0x7eed5835 => 124
	i32 2139458754, ; 133: Xamarin.Android.Support.DrawerLayout => 0x7f858cc2 => 54
	i32 2166116741, ; 134: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 50
	i32 2196165013, ; 135: Xamarin.Android.Support.VersionedParcelable => 0x82e6d195 => 66
	i32 2201231467, ; 136: System.Net.Http => 0x8334206b => 29
	i32 2217644978, ; 137: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 106
	i32 2244775296, ; 138: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 97
	i32 2256548716, ; 139: Xamarin.AndroidX.MultiDex => 0x8680336c => 99
	i32 2261435625, ; 140: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 89
	i32 2279755925, ; 141: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 101
	i32 2315684594, ; 142: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 69
	i32 2330457430, ; 143: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 49
	i32 2330986192, ; 144: Xamarin.Android.Support.SlidingPaneLayout.dll => 0x8af006d0 => 61
	i32 2373288475, ; 145: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 55
	i32 2435904999, ; 146: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 138
	i32 2440966767, ; 147: Xamarin.Android.Support.Loader.dll => 0x917e326f => 57
	i32 2465273461, ; 148: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 21
	i32 2471841756, ; 149: netstandard.dll => 0x93554fdc => 1
	i32 2475788418, ; 150: Java.Interop.dll => 0x93918882 => 7
	i32 2483661569, ; 151: Xamarin.Firebase.Measurement.Connector => 0x9409ab01 => 115
	i32 2483742551, ; 152: Xamarin.Firebase.Messaging => 0x940ae757 => 116
	i32 2487632829, ; 153: Xamarin.Android.Support.DocumentFile => 0x944643bd => 53
	i32 2501346920, ; 154: System.Data.DataSetExtensions => 0x95178668 => 130
	i32 2505896520, ; 155: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 93
	i32 2555655126, ; 156: Plugin.LocalNotifications.Abstractions.dll => 0x985433d6 => 15
	i32 2581819634, ; 157: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 107
	i32 2620871830, ; 158: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 81
	i32 2633051222, ; 159: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 92
	i32 2698266930, ; 160: Xamarin.Android.Arch.Lifecycle.LiveData => 0xa0d44932 => 40
	i32 2732626843, ; 161: Xamarin.AndroidX.Activity => 0xa2e0939b => 68
	i32 2737747696, ; 162: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 71
	i32 2751899777, ; 163: Xamarin.Android.Support.Collections => 0xa406a881 => 46
	i32 2766581644, ; 164: Xamarin.Forms.Core => 0xa4e6af8c => 117
	i32 2768457651, ; 165: PropertyChanged => 0xa5034fb3 => 18
	i32 2778768386, ; 166: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 109
	i32 2788639665, ; 167: Xamarin.Android.Support.LocalBroadcastManager => 0xa63743b1 => 58
	i32 2788775637, ; 168: Xamarin.Android.Support.SwipeRefreshLayout.dll => 0xa63956d5 => 62
	i32 2806986428, ; 169: Plugin.CurrentActivity.dll => 0xa74f36bc => 13
	i32 2810250172, ; 170: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 79
	i32 2819470561, ; 171: System.Xml.dll => 0xa80db4e1 => 34
	i32 2843355708, ; 172: Lottie.Android.dll => 0xa97a2a3c => 8
	i32 2847418871, ; 173: Xamarin.GooglePlayServices.Base => 0xa9b829f7 => 124
	i32 2853208004, ; 174: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 109
	i32 2855708567, ; 175: Xamarin.AndroidX.Transition => 0xaa36a797 => 105
	i32 2861816565, ; 176: Rg.Plugins.Popup.dll => 0xaa93daf5 => 19
	i32 2870995654, ; 177: Xamarin.Firebase.Iid => 0xab1feac6 => 113
	i32 2876493027, ; 178: Xamarin.Android.Support.SwipeRefreshLayout => 0xab73cce3 => 62
	i32 2893257763, ; 179: Xamarin.Android.Arch.Core.Runtime.dll => 0xac739c23 => 37
	i32 2903344695, ; 180: System.ComponentModel.Composition => 0xad0d8637 => 134
	i32 2905242038, ; 181: mscorlib.dll => 0xad2a79b6 => 11
	i32 2914202368, ; 182: Xamarin.Firebase.Iid.Interop => 0xadb33300 => 114
	i32 2916838712, ; 183: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 110
	i32 2919462931, ; 184: System.Numerics.Vectors.dll => 0xae037813 => 31
	i32 2921128767, ; 185: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 70
	i32 2921692953, ; 186: Xamarin.Android.Support.CustomView.dll => 0xae257f19 => 52
	i32 2922925221, ; 187: Xamarin.Android.Support.Vector.Drawable.dll => 0xae384ca5 => 65
	i32 2978675010, ; 188: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 84
	i32 3024354802, ; 189: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 88
	i32 3044182254, ; 190: FormsViewGroup => 0xb57288ee => 6
	i32 3056250942, ; 191: Xamarin.Android.Support.AsyncLayoutInflater.dll => 0xb62ab03e => 45
	i32 3058099980, ; 192: Xamarin.GooglePlayServices.Tasks => 0xb646e70c => 127
	i32 3068715062, ; 193: Xamarin.Android.Arch.Lifecycle.Common => 0xb6e8e036 => 38
	i32 3071899978, ; 194: Xamarin.Firebase.Common.dll => 0xb719794a => 112
	i32 3092211740, ; 195: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 59
	i32 3111772706, ; 196: System.Runtime.Serialization => 0xb979e222 => 2
	i32 3155965891, ; 197: Church.Mobile => 0xbc1c37c3 => 5
	i32 3178517120, ; 198: Plugin.LocalNotifications.dll => 0xbd745280 => 16
	i32 3204380047, ; 199: System.Data.dll => 0xbefef58f => 128
	i32 3204912593, ; 200: Xamarin.Android.Support.AsyncLayoutInflater => 0xbf0715d1 => 45
	i32 3211777861, ; 201: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 83
	i32 3230466174, ; 202: Xamarin.GooglePlayServices.Basement.dll => 0xc08d007e => 125
	i32 3233339011, ; 203: Xamarin.Android.Arch.Lifecycle.LiveData.Core.dll => 0xc0b8d683 => 39
	i32 3247949154, ; 204: Mono.Security => 0xc197c562 => 139
	i32 3258312781, ; 205: Xamarin.AndroidX.CardView => 0xc235e84d => 77
	i32 3263631797, ; 206: Lottie.Forms => 0xc28711b5 => 9
	i32 3267021929, ; 207: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 75
	i32 3286872994, ; 208: SQLite-net.dll => 0xc3e9b3a2 => 20
	i32 3296380511, ; 209: Xamarin.Android.Support.Collections.dll => 0xc47ac65f => 46
	i32 3317135071, ; 210: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 82
	i32 3317144872, ; 211: System.Data => 0xc5b79d28 => 128
	i32 3321585405, ; 212: Xamarin.Android.Support.DocumentFile.dll => 0xc5fb5efd => 53
	i32 3331531814, ; 213: Xamarin.GooglePlayServices.Stats.dll => 0xc6932426 => 126
	i32 3340431453, ; 214: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 74
	i32 3352662376, ; 215: Xamarin.Android.Support.CoordinaterLayout => 0xc7d59168 => 48
	i32 3353484488, ; 216: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 87
	i32 3357663996, ; 217: Xamarin.Android.Support.CursorAdapter => 0xc821e2fc => 51
	i32 3360279109, ; 218: SQLitePCLRaw.core => 0xc849ca45 => 22
	i32 3362522851, ; 219: Xamarin.AndroidX.Core => 0xc86c06e3 => 80
	i32 3366347497, ; 220: Java.Interop => 0xc8a662e9 => 7
	i32 3374999561, ; 221: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 101
	i32 3395150330, ; 222: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 32
	i32 3401559547, ; 223: Plugin.FirebasePushNotification.dll => 0xcabfadfb => 14
	i32 3404865022, ; 224: System.ServiceModel.Internals => 0xcaf21dfe => 136
	i32 3429136800, ; 225: System.Xml => 0xcc6479a0 => 34
	i32 3430777524, ; 226: netstandard => 0xcc7d82b4 => 1
	i32 3439690031, ; 227: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 44
	i32 3476120550, ; 228: Mono.Android => 0xcf3163e6 => 10
	i32 3486566296, ; 229: System.Transactions => 0xcfd0c798 => 129
	i32 3501239056, ; 230: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 75
	i32 3509114376, ; 231: System.Xml.Linq => 0xd128d608 => 35
	i32 3536029504, ; 232: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 118
	i32 3541357780, ; 233: Church.Mobile.dll => 0xd314d4d4 => 5
	i32 3547625832, ; 234: Xamarin.Android.Support.SlidingPaneLayout => 0xd3747968 => 61
	i32 3548701333, ; 235: Church.Mobile.Android.dll => 0xd384e295 => 0
	i32 3567349600, ; 236: System.ComponentModel.Composition.dll => 0xd4a16f60 => 134
	i32 3627220390, ; 237: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 100
	i32 3632359727, ; 238: Xamarin.Forms.Platform => 0xd881692f => 119
	i32 3633644679, ; 239: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 70
	i32 3639449509, ; 240: Lottie.Android => 0xd8ed97a5 => 8
	i32 3641597786, ; 241: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 91
	i32 3645089577, ; 242: System.ComponentModel.DataAnnotations => 0xd943a729 => 138
	i32 3664423555, ; 243: Xamarin.Android.Support.ViewPager.dll => 0xda6aaa83 => 67
	i32 3672681054, ; 244: Mono.Android.dll => 0xdae8aa5e => 10
	i32 3676310014, ; 245: System.Web.Services.dll => 0xdb2009fe => 135
	i32 3678221644, ; 246: Xamarin.Android.Support.v7.AppCompat => 0xdb3d354c => 64
	i32 3681174138, ; 247: Xamarin.Android.Arch.Lifecycle.Common.dll => 0xdb6a427a => 38
	i32 3682565725, ; 248: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 76
	i32 3689375977, ; 249: System.Drawing.Common => 0xdbe768e9 => 131
	i32 3714038699, ; 250: Xamarin.Android.Support.LocalBroadcastManager.dll => 0xdd5fbbab => 58
	i32 3718463572, ; 251: Xamarin.Android.Support.Animated.Vector.Drawable.dll => 0xdda34054 => 43
	i32 3718780102, ; 252: Xamarin.AndroidX.Annotation => 0xdda814c6 => 69
	i32 3754567612, ; 253: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 24
	i32 3758932259, ; 254: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 89
	i32 3776062606, ; 255: Xamarin.Android.Support.DrawerLayout.dll => 0xe112248e => 54
	i32 3786282454, ; 256: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 78
	i32 3822602673, ; 257: Xamarin.AndroidX.Media => 0xe3d849b1 => 98
	i32 3829621856, ; 258: System.Numerics.dll => 0xe4436460 => 30
	i32 3832731800, ; 259: Xamarin.Android.Support.CoordinaterLayout.dll => 0xe472d898 => 48
	i32 3862817207, ; 260: Xamarin.Android.Arch.Lifecycle.Runtime.dll => 0xe63de9b7 => 41
	i32 3874897629, ; 261: Xamarin.Android.Arch.Lifecycle.Runtime => 0xe6f63edd => 41
	i32 3876362041, ; 262: SQLite-net => 0xe70c9739 => 20
	i32 3883175360, ; 263: Xamarin.Android.Support.v7.AppCompat.dll => 0xe7748dc0 => 64
	i32 3885922214, ; 264: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 105
	i32 3896760992, ; 265: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 80
	i32 3920810846, ; 266: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 133
	i32 3921031405, ; 267: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 108
	i32 3945713374, ; 268: System.Data.DataSetExtensions.dll => 0xeb2ecede => 130
	i32 3955647286, ; 269: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 72
	i32 3970018735, ; 270: Xamarin.GooglePlayServices.Tasks.dll => 0xeca1adaf => 127
	i32 4025784931, ; 271: System.Memory => 0xeff49a63 => 28
	i32 4063435586, ; 272: Xamarin.Android.Support.CustomView => 0xf2331b42 => 52
	i32 4105002889, ; 273: Mono.Security.dll => 0xf4ad5f89 => 139
	i32 4151237749, ; 274: System.Core => 0xf76edc75 => 26
	i32 4182413190, ; 275: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 95
	i32 4184000013, ; 276: PropertyChanged.dll => 0xf962c60d => 18
	i32 4260525087, ; 277: System.Buffers => 0xfdf2741f => 25
	i32 4268646418, ; 278: CardView.dll => 0xfe6e6012 => 4
	i32 4292120959 ; 279: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 95
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [280 x i32] [
	i32 93, i32 123, i32 12, i32 17, i32 117, i32 66, i32 113, i32 103, ; 0..7
	i32 103, i32 9, i32 36, i32 78, i32 47, i32 104, i32 37, i32 76, ; 8..15
	i32 63, i32 88, i32 135, i32 92, i32 86, i32 68, i32 30, i32 16, ; 16..23
	i32 90, i32 23, i32 28, i32 43, i32 49, i32 111, i32 85, i32 11, ; 24..31
	i32 27, i32 86, i32 97, i32 47, i32 60, i32 129, i32 126, i32 137, ; 32..39
	i32 133, i32 82, i32 108, i32 73, i32 35, i32 44, i32 21, i32 132, ; 40..47
	i32 122, i32 56, i32 131, i32 100, i32 63, i32 19, i32 60, i32 123, ; 48..55
	i32 12, i32 13, i32 59, i32 90, i32 6, i32 40, i32 102, i32 72, ; 56..63
	i32 119, i32 94, i32 122, i32 42, i32 27, i32 17, i32 115, i32 106, ; 64..71
	i32 4, i32 73, i32 107, i32 22, i32 51, i32 84, i32 39, i32 14, ; 72..79
	i32 112, i32 136, i32 102, i32 3, i32 98, i32 79, i32 32, i32 55, ; 80..87
	i32 137, i32 120, i32 132, i32 71, i32 114, i32 50, i32 83, i32 36, ; 88..95
	i32 2, i32 96, i32 110, i32 81, i32 29, i32 33, i32 121, i32 65, ; 96..103
	i32 23, i32 77, i32 104, i32 26, i32 85, i32 96, i32 121, i32 15, ; 104..111
	i32 56, i32 111, i32 120, i32 74, i32 0, i32 3, i32 125, i32 42, ; 112..119
	i32 99, i32 116, i32 25, i32 94, i32 67, i32 57, i32 91, i32 33, ; 120..127
	i32 31, i32 87, i32 24, i32 118, i32 124, i32 54, i32 50, i32 66, ; 128..135
	i32 29, i32 106, i32 97, i32 99, i32 89, i32 101, i32 69, i32 49, ; 136..143
	i32 61, i32 55, i32 138, i32 57, i32 21, i32 1, i32 7, i32 115, ; 144..151
	i32 116, i32 53, i32 130, i32 93, i32 15, i32 107, i32 81, i32 92, ; 152..159
	i32 40, i32 68, i32 71, i32 46, i32 117, i32 18, i32 109, i32 58, ; 160..167
	i32 62, i32 13, i32 79, i32 34, i32 8, i32 124, i32 109, i32 105, ; 168..175
	i32 19, i32 113, i32 62, i32 37, i32 134, i32 11, i32 114, i32 110, ; 176..183
	i32 31, i32 70, i32 52, i32 65, i32 84, i32 88, i32 6, i32 45, ; 184..191
	i32 127, i32 38, i32 112, i32 59, i32 2, i32 5, i32 16, i32 128, ; 192..199
	i32 45, i32 83, i32 125, i32 39, i32 139, i32 77, i32 9, i32 75, ; 200..207
	i32 20, i32 46, i32 82, i32 128, i32 53, i32 126, i32 74, i32 48, ; 208..215
	i32 87, i32 51, i32 22, i32 80, i32 7, i32 101, i32 32, i32 14, ; 216..223
	i32 136, i32 34, i32 1, i32 44, i32 10, i32 129, i32 75, i32 35, ; 224..231
	i32 118, i32 5, i32 61, i32 0, i32 134, i32 100, i32 119, i32 70, ; 232..239
	i32 8, i32 91, i32 138, i32 67, i32 10, i32 135, i32 64, i32 38, ; 240..247
	i32 76, i32 131, i32 58, i32 43, i32 69, i32 24, i32 89, i32 54, ; 248..255
	i32 78, i32 98, i32 30, i32 48, i32 41, i32 41, i32 20, i32 64, ; 256..263
	i32 105, i32 80, i32 133, i32 108, i32 130, i32 72, i32 127, i32 28, ; 264..271
	i32 52, i32 139, i32 26, i32 95, i32 18, i32 25, i32 4, i32 95 ; 280..279
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ a8a26c7b003e2524cc98acb2c2ffc2ddea0f6692"}
!llvm.linker.options = !{}
