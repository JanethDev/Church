; ModuleID = 'obj\Debug\110\android\marshal_methods.arm64-v8a.ll'
source_filename = "obj\Debug\110\android\marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android"


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
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 8
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [280 x i64] [
	i64 120698629574877762, ; 0: Mono.Android => 0x1accec39cafe242 => 10
	i64 210515253464952879, ; 1: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 78
	i64 232391251801502327, ; 2: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 102
	i64 263803244706540312, ; 3: Rg.Plugins.Popup.dll => 0x3a937a743542b18 => 19
	i64 295915112840604065, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 103
	i64 421225479760926298, ; 5: Plugin.LocalNotifications.Abstractions => 0x5d87e57937e565a => 15
	i64 590536689428908136, ; 6: Xamarin.Android.Arch.Lifecycle.ViewModel.dll => 0x83201fd803ec868 => 42
	i64 634308326490598313, ; 7: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 93
	i64 687654259221141486, ; 8: Xamarin.GooglePlayServices.Base => 0x98b09e7c92917ee => 124
	i64 702024105029695270, ; 9: System.Drawing.Common => 0x9be17343c0e7726 => 131
	i64 720058930071658100, ; 10: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x9fe29c82844de74 => 87
	i64 816102801403336439, ; 11: Xamarin.Android.Support.Collections => 0xb53612c89d8d6f7 => 46
	i64 846634227898301275, ; 12: Xamarin.Android.Arch.Lifecycle.LiveData.Core => 0xbbfd9583890bb5b => 39
	i64 870603111519317375, ; 13: SQLitePCLRaw.lib.e_sqlite3.android => 0xc1500ead2756d7f => 23
	i64 872800313462103108, ; 14: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 84
	i64 940822596282819491, ; 15: System.Transactions => 0xd0e792aa81923a3 => 129
	i64 996343623809489702, ; 16: Xamarin.Forms.Platform => 0xdd3b93f3b63db26 => 119
	i64 1000557547492888992, ; 17: Mono.Security.dll => 0xde2b1c9cba651a0 => 139
	i64 1120440138749646132, ; 18: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 121
	i64 1301485588176585670, ; 19: SQLitePCLRaw.core => 0x120fce3f338e43c6 => 22
	i64 1315114680217950157, ; 20: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 73
	i64 1342439039765371018, ; 21: Xamarin.Android.Support.Fragment => 0x12a14d31b1d4d88a => 55
	i64 1400031058434376889, ; 22: Plugin.Permissions.dll => 0x136de8d4787ec4b9 => 17
	i64 1425944114962822056, ; 23: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 2
	i64 1490981186906623721, ; 24: Xamarin.Android.Support.VersionedParcelable => 0x14b1077d6c5c0ee9 => 66
	i64 1491290866305144020, ; 25: Xamarin.Google.AutoValue.Annotations.dll => 0x14b2212446e714d4 => 122
	i64 1518315023656898250, ; 26: SQLitePCLRaw.provider.e_sqlite3 => 0x151223783a354eca => 24
	i64 1609086626780420733, ; 27: Plugin.LocalNotifications.dll => 0x16549fc302141a7d => 16
	i64 1624659445732251991, ; 28: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 71
	i64 1628611045998245443, ; 29: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 95
	i64 1636321030536304333, ; 30: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 88
	i64 1731380447121279447, ; 31: Newtonsoft.Json => 0x18071957e9b889d7 => 12
	i64 1743969030606105336, ; 32: System.Memory.dll => 0x1833d297e88f2af8 => 28
	i64 1744702963312407042, ; 33: Xamarin.Android.Support.v7.AppCompat => 0x18366e19eeceb202 => 64
	i64 1795316252682057001, ; 34: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 72
	i64 1836611346387731153, ; 35: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 102
	i64 1875917498431009007, ; 36: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 69
	i64 1938067011858688285, ; 37: Xamarin.Android.Support.v4.dll => 0x1ae565add0bd691d => 63
	i64 1981742497975770890, ; 38: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 94
	i64 2133195048986300728, ; 39: Newtonsoft.Json.dll => 0x1d9aa1984b735138 => 12
	i64 2136356949452311481, ; 40: Xamarin.AndroidX.MultiDex.dll => 0x1da5dd539d8acbb9 => 99
	i64 2137969380975227603, ; 41: PropertyChanged => 0x1dab97d315b0b2d3 => 18
	i64 2165725771938924357, ; 42: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 76
	i64 2262844636196693701, ; 43: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 84
	i64 2284400282711631002, ; 44: System.Web.Services => 0x1fb3d1f42fd4249a => 135
	i64 2287887973817120656, ; 45: System.ComponentModel.DataAnnotations.dll => 0x1fc035fd8d41f790 => 138
	i64 2329709569556905518, ; 46: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 91
	i64 2337758774805907496, ; 47: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 32
	i64 2470498323731680442, ; 48: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 79
	i64 2479423007379663237, ; 49: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 106
	i64 2497223385847772520, ; 50: System.Runtime => 0x22a7eb7046413568 => 33
	i64 2541787113603107559, ; 51: Lottie.Android.dll => 0x23463de9b0fa8ae7 => 8
	i64 2547086958574651984, ; 52: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 68
	i64 2592350477072141967, ; 53: System.Xml.dll => 0x23f9e10627330e8f => 34
	i64 2624866290265602282, ; 54: mscorlib.dll => 0x246d65fbde2db8ea => 11
	i64 2783046991838674048, ; 55: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 32
	i64 2801558180824670388, ; 56: Plugin.CurrentActivity.dll => 0x26e1225279a4e0b4 => 13
	i64 2949706848458024531, ; 57: Xamarin.Android.Support.SlidingPaneLayout => 0x28ef76c01de0a653 => 61
	i64 2960931600190307745, ; 58: Xamarin.Forms.Core => 0x2917579a49927da1 => 117
	i64 2977248461349026546, ; 59: Xamarin.Android.Support.DrawerLayout => 0x29514fb392c97af2 => 54
	i64 3017704767998173186, ; 60: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 121
	i64 3022227708164871115, ; 61: Xamarin.Android.Support.Media.Compat.dll => 0x29f11c168f8293cb => 59
	i64 3260998928894807349, ; 62: Lottie.Forms.dll => 0x2d41653f91b44d35 => 9
	i64 3289520064315143713, ; 63: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 90
	i64 3303437397778967116, ; 64: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 70
	i64 3311221304742556517, ; 65: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 31
	i64 3364695309916733813, ; 66: Xamarin.Firebase.Common => 0x2eb1cc8eb5028175 => 112
	i64 3411255996856937470, ; 67: Xamarin.GooglePlayServices.Basement => 0x2f5737416a942bfe => 125
	i64 3503068555325934332, ; 68: CardView => 0x309d664a800b0afc => 4
	i64 3522470458906976663, ; 69: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 104
	i64 3531994851595924923, ; 70: System.Numerics => 0x31042a9aade235bb => 30
	i64 3571415421602489686, ; 71: System.Runtime.dll => 0x319037675df7e556 => 33
	i64 3609787854626478660, ; 72: Plugin.CurrentActivity => 0x32188aeda587da44 => 13
	i64 3716579019761409177, ; 73: netstandard.dll => 0x3393f0ed5c8c5c99 => 1
	i64 3727469159507183293, ; 74: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 101
	i64 3966267475168208030, ; 75: System.Memory => 0x370b03412596249e => 28
	i64 4247996603072512073, ; 76: Xamarin.GooglePlayServices.Tasks => 0x3af3ea6755340049 => 127
	i64 4252163538099460320, ; 77: Xamarin.Android.Support.ViewPager.dll => 0x3b02b8357f4958e0 => 67
	i64 4337444564132831293, ; 78: SQLitePCLRaw.batteries_v2.dll => 0x3c31b2d9ae16203d => 21
	i64 4349341163892612332, ; 79: Xamarin.Android.Support.DocumentFile => 0x3c5bf6bea8cd9cec => 53
	i64 4416987920449902723, ; 80: Xamarin.Android.Support.AsyncLayoutInflater.dll => 0x3d4c4b1c879b9883 => 45
	i64 4525561845656915374, ; 81: System.ServiceModel.Internals => 0x3ece06856b710dae => 136
	i64 4636684751163556186, ; 82: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 108
	i64 4782108999019072045, ; 83: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0x425d76cc43bb0a2d => 75
	i64 4794310189461587505, ; 84: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 68
	i64 4795410492532947900, ; 85: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 104
	i64 4841234827713643511, ; 86: Xamarin.Android.Support.CoordinaterLayout => 0x432f856d041f03f7 => 48
	i64 4963205065368577818, ; 87: Xamarin.Android.Support.LocalBroadcastManager.dll => 0x44e0d8b5f4b6a71a => 58
	i64 5081566143765835342, ; 88: System.Resources.ResourceManager.dll => 0x4685597c05d06e4e => 3
	i64 5099468265966638712, ; 89: System.Resources.ResourceManager => 0x46c4f35ea8519678 => 3
	i64 5142919913060024034, ; 90: Xamarin.Forms.Platform.Android.dll => 0x475f52699e39bee2 => 118
	i64 5178572682164047940, ; 91: Xamarin.Android.Support.Print.dll => 0x47ddfc6acbee1044 => 60
	i64 5203618020066742981, ; 92: Xamarin.Essentials => 0x4836f704f0e652c5 => 111
	i64 5205316157927637098, ; 93: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 97
	i64 5288341611614403055, ; 94: Xamarin.Android.Support.Interpolator.dll => 0x4963f6ad4b3179ef => 56
	i64 5348796042099802469, ; 95: Xamarin.AndroidX.Media => 0x4a3abda9415fc165 => 98
	i64 5376510917114486089, ; 96: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 106
	i64 5408338804355907810, ; 97: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 105
	i64 5439315836349573567, ; 98: Xamarin.Android.Support.Animated.Vector.Drawable.dll => 0x4b7c54ef36c5e9bf => 43
	i64 5507995362134886206, ; 99: System.Core.dll => 0x4c705499688c873e => 26
	i64 5692067934154308417, ; 100: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 110
	i64 5767696078500135884, ; 101: Xamarin.Android.Support.Annotations.dll => 0x500af9065b6a03cc => 44
	i64 5896680224035167651, ; 102: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 92
	i64 6039900244750604534, ; 103: CardView.dll => 0x53d20945972798f6 => 4
	i64 6044705416426755068, ; 104: Xamarin.Android.Support.SwipeRefreshLayout.dll => 0x53e31b8ccdff13fc => 62
	i64 6085203216496545422, ; 105: Xamarin.Forms.Platform.dll => 0x5472fc15a9574e8e => 119
	i64 6086316965293125504, ; 106: FormsViewGroup.dll => 0x5476f10882baef80 => 6
	i64 6092862891035488599, ; 107: Xamarin.Firebase.Measurement.Connector.dll => 0x548e32849d547157 => 115
	i64 6183170893902868313, ; 108: SQLitePCLRaw.batteries_v2 => 0x55cf092b0c9d6f59 => 21
	i64 6300241346327543539, ; 109: Xamarin.Firebase.Iid => 0x576ef41fd714fef3 => 113
	i64 6311200438583329442, ; 110: Xamarin.Android.Support.LocalBroadcastManager => 0x5795e35c580c82a2 => 58
	i64 6319713645133255417, ; 111: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 93
	i64 6401687960814735282, ; 112: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 91
	i64 6405879832841858445, ; 113: Xamarin.Android.Support.Vector.Drawable.dll => 0x58e641c4a660ad8d => 65
	i64 6504860066809920875, ; 114: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 76
	i64 6548213210057960872, ; 115: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 82
	i64 6554405243736097249, ; 116: Xamarin.GooglePlayServices.Stats => 0x5af5ecd7aad901e1 => 126
	i64 6588599331800941662, ; 117: Xamarin.Android.Support.v4 => 0x5b6f682f335f045e => 63
	i64 6591024623626361694, ; 118: System.Web.Services.dll => 0x5b7805f9751a1b5e => 135
	i64 6659513131007730089, ; 119: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0x5c6b57e8b6c3e1a9 => 87
	i64 6876862101832370452, ; 120: System.Xml.Linq => 0x5f6f85a57d108914 => 35
	i64 6894844156784520562, ; 121: System.Numerics.Vectors => 0x5faf683aead1ad72 => 31
	i64 7036436454368433159, ; 122: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x61a671acb33d5407 => 89
	i64 7103753931438454322, ; 123: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 86
	i64 7194160955514091247, ; 124: Xamarin.Android.Support.CursorAdapter.dll => 0x63d6cb45d266f6ef => 51
	i64 7385250113861300937, ; 125: Xamarin.Firebase.Iid.Interop.dll => 0x667dadd98e1db2c9 => 114
	i64 7488575175965059935, ; 126: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 35
	i64 7635363394907363464, ; 127: Xamarin.Forms.Core.dll => 0x69f6428dc4795888 => 117
	i64 7637365915383206639, ; 128: Xamarin.Essentials.dll => 0x69fd5fd5e61792ef => 111
	i64 7654504624184590948, ; 129: System.Net.Http => 0x6a3a4366801b8264 => 29
	i64 7756332380610150586, ; 130: Xamarin.Google.AutoValue.Annotations => 0x6ba407349220c0ba => 122
	i64 7820441508502274321, ; 131: System.Data => 0x6c87ca1e14ff8111 => 128
	i64 7821246742157274664, ; 132: Xamarin.Android.Support.AsyncLayoutInflater => 0x6c8aa67926f72e28 => 45
	i64 7836164640616011524, ; 133: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 71
	i64 7879037620440914030, ; 134: Xamarin.Android.Support.v7.AppCompat.dll => 0x6d57f6f88a51d86e => 64
	i64 8044118961405839122, ; 135: System.ComponentModel.Composition => 0x6fa2739369944712 => 134
	i64 8083354569033831015, ; 136: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 90
	i64 8101777744205214367, ; 137: Xamarin.Android.Support.Annotations => 0x706f4beeec84729f => 44
	i64 8103644804370223335, ; 138: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 130
	i64 8167236081217502503, ; 139: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 7
	i64 8196541262927413903, ; 140: Xamarin.Android.Support.Interpolator => 0x71bff6d9fb9ec28f => 56
	i64 8290740647658429042, ; 141: System.Runtime.Extensions => 0x730ea0b15c929a72 => 137
	i64 8318905602908530212, ; 142: System.ComponentModel.DataAnnotations => 0x7372b092055ea624 => 138
	i64 8385935383968044654, ; 143: Xamarin.Android.Arch.Lifecycle.Runtime.dll => 0x7460d3cd16cb566e => 41
	i64 8428678171113854126, ; 144: Xamarin.Firebase.Iid.dll => 0x74f8ae23bb5494ae => 113
	i64 8465511506719290632, ; 145: Xamarin.Firebase.Messaging.dll => 0x757b89dcf7fc3508 => 116
	i64 8601935802264776013, ; 146: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 105
	i64 8603079619611647627, ; 147: Church.Mobile.dll => 0x776447553d5ade8b => 5
	i64 8626175481042262068, ; 148: Java.Interop => 0x77b654e585b55834 => 7
	i64 8684531736582871431, ; 149: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 133
	i64 8808820144457481518, ; 150: Xamarin.Android.Support.Loader.dll => 0x7a3f374010b17d2e => 57
	i64 8844506025403580595, ; 151: Plugin.FirebasePushNotification => 0x7abdff5eb1fb80b3 => 14
	i64 8917102979740339192, ; 152: Xamarin.Android.Support.DocumentFile.dll => 0x7bbfe9ea4d000bf8 => 53
	i64 9312692141327339315, ; 153: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 110
	i64 9324707631942237306, ; 154: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 72
	i64 9662334977499516867, ; 155: System.Numerics.dll => 0x8617827802b0cfc3 => 30
	i64 9678050649315576968, ; 156: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 79
	i64 9704315356731487263, ; 157: Plugin.FirebasePushNotification.dll => 0x86aca766ba59341f => 14
	i64 9711637524876806384, ; 158: Xamarin.AndroidX.Media.dll => 0x86c6aadfd9a2c8f0 => 98
	i64 9769999157949715138, ; 159: Plugin.LocalNotifications.Abstractions.dll => 0x87960278717552c2 => 15
	i64 9796610708422913120, ; 160: Xamarin.Firebase.Iid.Interop => 0x87f48d88de55ec60 => 114
	i64 9808709177481450983, ; 161: Mono.Android.dll => 0x881f890734e555e7 => 10
	i64 9834056768316610435, ; 162: System.Transactions.dll => 0x8879968718899783 => 129
	i64 9866412715007501892, ; 163: Xamarin.Android.Arch.Lifecycle.Common.dll => 0x88ec8a16fd6b6644 => 38
	i64 9875200773399460291, ; 164: Xamarin.GooglePlayServices.Base.dll => 0x890bc2c8482339c3 => 124
	i64 9998632235833408227, ; 165: Mono.Security => 0x8ac2470b209ebae3 => 139
	i64 10038780035334861115, ; 166: System.Net.Http.dll => 0x8b50e941206af13b => 29
	i64 10229024438826829339, ; 167: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 82
	i64 10303855825347935641, ; 168: Xamarin.Android.Support.Loader => 0x8efea647eeb3fd99 => 57
	i64 10352330178246763130, ; 169: Xamarin.Firebase.Measurement.Connector => 0x8faadd72b7f4627a => 115
	i64 10363495123250631811, ; 170: Xamarin.Android.Support.Collections.dll => 0x8fd287e80cd8d483 => 46
	i64 10430153318873392755, ; 171: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 80
	i64 10635644668885628703, ; 172: Xamarin.Android.Support.DrawerLayout.dll => 0x93996679ee34771f => 54
	i64 10714184849103829812, ; 173: System.Runtime.Extensions.dll => 0x94b06e5aa4b4bb34 => 137
	i64 10847732767863316357, ; 174: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 73
	i64 10850923258212604222, ; 175: Xamarin.Android.Arch.Lifecycle.Runtime => 0x9696393672c9593e => 41
	i64 11023048688141570732, ; 176: System.Core => 0x98f9bc61168392ac => 26
	i64 11037814507248023548, ; 177: System.Xml => 0x992e31d0412bf7fc => 34
	i64 11162124722117608902, ; 178: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 109
	i64 11340910727871153756, ; 179: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 81
	i64 11376461258732682436, ; 180: Xamarin.Android.Support.Compat => 0x9de14f3d5fc13cc4 => 47
	i64 11392833485892708388, ; 181: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 100
	i64 11529969570048099689, ; 182: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 109
	i64 11578238080964724296, ; 183: Xamarin.AndroidX.Legacy.Support.V4 => 0xa0ae2a30c4cd8648 => 89
	i64 11580057168383206117, ; 184: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 69
	i64 11597940890313164233, ; 185: netstandard => 0xa0f429ca8d1805c9 => 1
	i64 11672361001936329215, ; 186: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 86
	i64 11739066727115742305, ; 187: SQLite-net.dll => 0xa2e98afdf8575c61 => 20
	i64 11806260347154423189, ; 188: SQLite-net => 0xa3d8433bc5eb5d95 => 20
	i64 11834399401546345650, ; 189: Xamarin.Android.Support.SlidingPaneLayout.dll => 0xa43c3b8deb43ecb2 => 61
	i64 11865714326292153359, ; 190: Xamarin.Android.Arch.Lifecycle.LiveData => 0xa4ab7c5000e8440f => 40
	i64 12102847907131387746, ; 191: System.Buffers => 0xa7f5f40c43256f62 => 25
	i64 12137774235383566651, ; 192: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 107
	i64 12246629560347601292, ; 193: Plugin.LocalNotifications => 0xa9f4c4b32051298c => 16
	i64 12279246230491828964, ; 194: SQLitePCLRaw.provider.e_sqlite3.dll => 0xaa68a5636e0512e4 => 24
	i64 12388767885335911387, ; 195: Xamarin.Android.Arch.Lifecycle.LiveData.dll => 0xabedbec0d236dbdb => 40
	i64 12414299427252656003, ; 196: Xamarin.Android.Support.Compat.dll => 0xac48738e28bad783 => 47
	i64 12451044538927396471, ; 197: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 85
	i64 12466513435562512481, ; 198: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 96
	i64 12487638416075308985, ; 199: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 83
	i64 12488608402635344228, ; 200: Lottie.Android => 0xad50732cba09c964 => 8
	i64 12538491095302438457, ; 201: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 77
	i64 12550732019250633519, ; 202: System.IO.Compression => 0xae2d28465e8e1b2f => 132
	i64 12700543734426720211, ; 203: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 78
	i64 12952608645614506925, ; 204: Xamarin.Android.Support.Core.Utils => 0xb3c0e8eff48193ad => 50
	i64 12963446364377008305, ; 205: System.Drawing.Common.dll => 0xb3e769c8fd8548b1 => 131
	i64 13358059602087096138, ; 206: Xamarin.Android.Support.Fragment.dll => 0xb9615c6f1ee5af4a => 55
	i64 13370592475155966277, ; 207: System.Runtime.Serialization => 0xb98de304062ea945 => 2
	i64 13401370062847626945, ; 208: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 107
	i64 13454009404024712428, ; 209: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 123
	i64 13491513212026656886, ; 210: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 74
	i64 13572454107664307259, ; 211: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 101
	i64 13647894001087880694, ; 212: System.Data.dll => 0xbd670f48cb071df6 => 128
	i64 13959074834287824816, ; 213: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 85
	i64 13967638549803255703, ; 214: Xamarin.Forms.Platform.Android => 0xc1d70541e0134797 => 118
	i64 14030805823765547820, ; 215: PropertyChanged.dll => 0xc2b76f8eee070b2c => 18
	i64 14124974489674258913, ; 216: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 77
	i64 14172845254133543601, ; 217: Xamarin.AndroidX.MultiDex => 0xc4b00faaed35f2b1 => 99
	i64 14261073672896646636, ; 218: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 100
	i64 14369828458497533121, ; 219: Xamarin.Android.Support.Vector.Drawable => 0xc76be2d9300b64c1 => 65
	i64 14400856865250966808, ; 220: Xamarin.Android.Support.Core.UI => 0xc7da1f051a877d18 => 49
	i64 14486659737292545672, ; 221: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 92
	i64 14644440854989303794, ; 222: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 97
	i64 14661790646341542033, ; 223: Xamarin.Android.Support.SwipeRefreshLayout => 0xcb7924e94e552091 => 62
	i64 14789919016435397935, ; 224: Xamarin.Firebase.Common.dll => 0xcd4058fc2f6d352f => 112
	i64 14792063746108907174, ; 225: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 123
	i64 14809388726477333247, ; 226: Xamarin.GooglePlayServices.Stats.dll => 0xcd8584954e5b22ff => 126
	i64 14852515768018889994, ; 227: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 81
	i64 14987728460634540364, ; 228: System.IO.Compression.dll => 0xcfff1ba06622494c => 132
	i64 14988210264188246988, ; 229: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 83
	i64 14998742490450220525, ; 230: Church.Mobile => 0xd0263cd40cd7d5ed => 5
	i64 15188640517174936311, ; 231: Xamarin.Android.Arch.Core.Common => 0xd2c8e413d75142f7 => 36
	i64 15246441518555807158, ; 232: Xamarin.Android.Arch.Core.Common.dll => 0xd3963dc832493db6 => 36
	i64 15326820765897713587, ; 233: Xamarin.Android.Arch.Core.Runtime.dll => 0xd4b3ce481769e7b3 => 37
	i64 15370334346939861994, ; 234: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 80
	i64 15457813392950723921, ; 235: Xamarin.Android.Support.Media.Compat => 0xd6852f61c31a8551 => 59
	i64 15568534730848034786, ; 236: Xamarin.Android.Support.VersionedParcelable.dll => 0xd80e8bda21875fe2 => 66
	i64 15582737692548360875, ; 237: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 95
	i64 15609085926864131306, ; 238: System.dll => 0xd89e9cf3334914ea => 27
	i64 15777549416145007739, ; 239: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 103
	i64 15810740023422282496, ; 240: Xamarin.Forms.Xaml => 0xdb6b08484c22eb00 => 120
	i64 15930129725311349754, ; 241: Xamarin.GooglePlayServices.Tasks.dll => 0xdd1330956f12f3fa => 127
	i64 16154507427712707110, ; 242: System => 0xe03056ea4e39aa26 => 27
	i64 16242842420508142678, ; 243: Xamarin.Android.Support.CoordinaterLayout.dll => 0xe16a2b1f8908ac56 => 48
	i64 16565028646146589191, ; 244: System.ComponentModel.Composition.dll => 0xe5e2cdc9d3bcc207 => 134
	i64 16716087184025001734, ; 245: Church.Mobile.Android.dll => 0xe7fb78ba6e451f06 => 0
	i64 16755018182064898362, ; 246: SQLitePCLRaw.core.dll => 0xe885c843c330813a => 22
	i64 16767985610513713374, ; 247: Xamarin.Android.Arch.Core.Runtime => 0xe8b3da12798808de => 37
	i64 16822611501064131242, ; 248: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 130
	i64 16833383113903931215, ; 249: mscorlib => 0xe99c30c1484d7f4f => 11
	i64 16895806301542741427, ; 250: Plugin.Permissions => 0xea79f6503d42f5b3 => 17
	i64 16932527889823454152, ; 251: Xamarin.Android.Support.Core.Utils.dll => 0xeafc6c67465253c8 => 50
	i64 17009591894298689098, ; 252: Xamarin.Android.Support.Animated.Vector.Drawable => 0xec0e35b50a097e4a => 43
	i64 17024911836938395553, ; 253: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 70
	i64 17037200463775726619, ; 254: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 88
	i64 17124705692820578889, ; 255: Lottie.Forms => 0xeda72d18d7ae2249 => 9
	i64 17285063141349522879, ; 256: Rg.Plugins.Popup => 0xefe0e158cc55fdbf => 19
	i64 17383232329670771222, ; 257: Xamarin.Android.Support.CustomView.dll => 0xf13da5b41a1cc216 => 52
	i64 17428701562824544279, ; 258: Xamarin.Android.Support.Core.UI.dll => 0xf1df2fbaec73d017 => 49
	i64 17483646997724851973, ; 259: Xamarin.Android.Support.ViewPager => 0xf2a2644fe5b6ef05 => 67
	i64 17524135665394030571, ; 260: Xamarin.Android.Support.Print => 0xf3323c8a739097eb => 60
	i64 17544493274320527064, ; 261: Xamarin.AndroidX.AsyncLayoutInflater => 0xf37a8fada41aded8 => 75
	i64 17666959971718154066, ; 262: Xamarin.Android.Support.CustomView => 0xf52da67d9f4e4752 => 52
	i64 17704177640604968747, ; 263: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 96
	i64 17710060891934109755, ; 264: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 94
	i64 17760961058993581169, ; 265: Xamarin.Android.Arch.Lifecycle.Common => 0xf67b9bfb46dbac71 => 38
	i64 17838668724098252521, ; 266: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 25
	i64 17841643939744178149, ; 267: Xamarin.Android.Arch.Lifecycle.ViewModel => 0xf79a40a25573dfe5 => 42
	i64 17882897186074144999, ; 268: FormsViewGroup => 0xf82cd03e3ac830e7 => 6
	i64 17892495832318972303, ; 269: Xamarin.Forms.Xaml.dll => 0xf84eea293687918f => 120
	i64 17928294245072900555, ; 270: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 133
	i64 17958105683855786126, ; 271: Xamarin.Android.Arch.Lifecycle.LiveData.Core.dll => 0xf93801f92d25c08e => 39
	i64 17986907704309214542, ; 272: Xamarin.GooglePlayServices.Basement.dll => 0xf99e554223166d4e => 125
	i64 18020738924664805166, ; 273: Church.Mobile.Android => 0xfa168692f2b3572e => 0
	i64 18116111925905154859, ; 274: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 74
	i64 18129453464017766560, ; 275: System.ServiceModel.Internals.dll => 0xfb98c1df1ec108a0 => 136
	i64 18301997741680159453, ; 276: Xamarin.Android.Support.CursorAdapter => 0xfdfdc1fa58d8eadd => 51
	i64 18337470502355292274, ; 277: Xamarin.Firebase.Messaging => 0xfe7bc8440c175072 => 116
	i64 18370042311372477656, ; 278: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0xfeef80274e4094d8 => 23
	i64 18380184030268848184 ; 279: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 108
], align 8
@assembly_image_cache_indices = local_unnamed_addr constant [280 x i32] [
	i32 10, i32 78, i32 102, i32 19, i32 103, i32 15, i32 42, i32 93, ; 0..7
	i32 124, i32 131, i32 87, i32 46, i32 39, i32 23, i32 84, i32 129, ; 8..15
	i32 119, i32 139, i32 121, i32 22, i32 73, i32 55, i32 17, i32 2, ; 16..23
	i32 66, i32 122, i32 24, i32 16, i32 71, i32 95, i32 88, i32 12, ; 24..31
	i32 28, i32 64, i32 72, i32 102, i32 69, i32 63, i32 94, i32 12, ; 32..39
	i32 99, i32 18, i32 76, i32 84, i32 135, i32 138, i32 91, i32 32, ; 40..47
	i32 79, i32 106, i32 33, i32 8, i32 68, i32 34, i32 11, i32 32, ; 48..55
	i32 13, i32 61, i32 117, i32 54, i32 121, i32 59, i32 9, i32 90, ; 56..63
	i32 70, i32 31, i32 112, i32 125, i32 4, i32 104, i32 30, i32 33, ; 64..71
	i32 13, i32 1, i32 101, i32 28, i32 127, i32 67, i32 21, i32 53, ; 72..79
	i32 45, i32 136, i32 108, i32 75, i32 68, i32 104, i32 48, i32 58, ; 80..87
	i32 3, i32 3, i32 118, i32 60, i32 111, i32 97, i32 56, i32 98, ; 88..95
	i32 106, i32 105, i32 43, i32 26, i32 110, i32 44, i32 92, i32 4, ; 96..103
	i32 62, i32 119, i32 6, i32 115, i32 21, i32 113, i32 58, i32 93, ; 104..111
	i32 91, i32 65, i32 76, i32 82, i32 126, i32 63, i32 135, i32 87, ; 112..119
	i32 35, i32 31, i32 89, i32 86, i32 51, i32 114, i32 35, i32 117, ; 120..127
	i32 111, i32 29, i32 122, i32 128, i32 45, i32 71, i32 64, i32 134, ; 128..135
	i32 90, i32 44, i32 130, i32 7, i32 56, i32 137, i32 138, i32 41, ; 136..143
	i32 113, i32 116, i32 105, i32 5, i32 7, i32 133, i32 57, i32 14, ; 144..151
	i32 53, i32 110, i32 72, i32 30, i32 79, i32 14, i32 98, i32 15, ; 152..159
	i32 114, i32 10, i32 129, i32 38, i32 124, i32 139, i32 29, i32 82, ; 160..167
	i32 57, i32 115, i32 46, i32 80, i32 54, i32 137, i32 73, i32 41, ; 168..175
	i32 26, i32 34, i32 109, i32 81, i32 47, i32 100, i32 109, i32 89, ; 176..183
	i32 69, i32 1, i32 86, i32 20, i32 20, i32 61, i32 40, i32 25, ; 184..191
	i32 107, i32 16, i32 24, i32 40, i32 47, i32 85, i32 96, i32 83, ; 192..199
	i32 8, i32 77, i32 132, i32 78, i32 50, i32 131, i32 55, i32 2, ; 200..207
	i32 107, i32 123, i32 74, i32 101, i32 128, i32 85, i32 118, i32 18, ; 208..215
	i32 77, i32 99, i32 100, i32 65, i32 49, i32 92, i32 97, i32 62, ; 216..223
	i32 112, i32 123, i32 126, i32 81, i32 132, i32 83, i32 5, i32 36, ; 224..231
	i32 36, i32 37, i32 80, i32 59, i32 66, i32 95, i32 27, i32 103, ; 232..239
	i32 120, i32 127, i32 27, i32 48, i32 134, i32 0, i32 22, i32 37, ; 240..247
	i32 130, i32 11, i32 17, i32 50, i32 43, i32 70, i32 88, i32 9, ; 248..255
	i32 19, i32 52, i32 49, i32 67, i32 60, i32 75, i32 52, i32 96, ; 256..263
	i32 94, i32 38, i32 25, i32 42, i32 6, i32 120, i32 133, i32 39, ; 264..271
	i32 125, i32 0, i32 74, i32 136, i32 51, i32 116, i32 23, i32 108 ; 280..279
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 8; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 8

; Function attributes: "frame-pointer"="non-leaf" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 8
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 8
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="non-leaf" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="non-leaf" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5}
!llvm.ident = !{!6}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"branch-target-enforcement", i32 0}
!3 = !{i32 1, !"sign-return-address", i32 0}
!4 = !{i32 1, !"sign-return-address-all", i32 0}
!5 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!6 = !{!"Xamarin.Android remotes/origin/d17-5 @ a8a26c7b003e2524cc98acb2c2ffc2ddea0f6692"}
!llvm.linker.options = !{}
