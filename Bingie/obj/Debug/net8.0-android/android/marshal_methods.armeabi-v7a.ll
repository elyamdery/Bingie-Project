; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [373 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [740 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 26230656, ; 3: Microsoft.Extensions.DependencyModel => 0x1903f80 => 194
	i32 32687329, ; 4: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 292
	i32 34715100, ; 5: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 326
	i32 34839235, ; 6: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39485524, ; 7: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 8: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 9: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 10: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 368
	i32 68219467, ; 11: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 12: Microsoft.Maui.Graphics.dll => 0x44bb714 => 221
	i32 82292897, ; 13: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 14: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 310
	i32 117431740, ; 15: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 16: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 310
	i32 122350210, ; 17: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 18: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 330
	i32 140305987, ; 19: Pomelo.EntityFrameworkCore.MySql.dll => 0x85ce643 => 224
	i32 142721839, ; 20: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 21: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 22: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 23: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 266
	i32 172961045, ; 24: Syncfusion.Maui.Core.dll => 0xa4f2d15 => 240
	i32 176265551, ; 25: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 26: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 312
	i32 184328833, ; 27: System.ValueTuple.dll => 0xafca281 => 151
	i32 189295616, ; 28: Microsoft.Kiota.Serialization.Text => 0xb486c00 => 215
	i32 195452805, ; 29: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 365
	i32 199333315, ; 30: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 366
	i32 205061960, ; 31: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 32: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 264
	i32 220171995, ; 33: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 34: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 286
	i32 230752869, ; 35: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 36: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 37: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 38: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 39: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 269
	i32 276479776, ; 40: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 41: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 288
	i32 280482487, ; 42: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 285
	i32 280992041, ; 43: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 337
	i32 291076382, ; 44: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 45: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 46: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 365
	i32 318968648, ; 47: Xamarin.AndroidX.Activity.dll => 0x13031348 => 255
	i32 321597661, ; 48: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 49: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 350
	i32 342366114, ; 50: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 287
	i32 347068432, ; 51: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 234
	i32 356389973, ; 52: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 349
	i32 360082299, ; 53: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 54: System.IO.Pipes => 0x15ebe147 => 55
	i32 374376850, ; 55: Syncfusion.Maui.Popup.dll => 0x16508992 => 241
	i32 374914964, ; 56: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 57: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 58: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 59: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 60: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 61: _Microsoft.Android.Resource.Designer => 0x17969339 => 369
	i32 398680804, ; 62: Serilog.Sinks.Console => 0x17c362e4 => 228
	i32 403441872, ; 63: WindowsBase => 0x180c08d0 => 165
	i32 435591531, ; 64: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 361
	i32 441335492, ; 65: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 270
	i32 442565967, ; 66: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 67: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 283
	i32 451504562, ; 68: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 69: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 70: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 71: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 72: System.dll => 0x1bff388e => 164
	i32 476646585, ; 73: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 285
	i32 485463106, ; 74: Microsoft.IdentityModel.Abstractions => 0x1cef9442 => 202
	i32 486930444, ; 75: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 298
	i32 498788369, ; 76: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 77: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 348
	i32 503918385, ; 78: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 342
	i32 506800726, ; 79: Microsoft.Kiota.Serialization.Text.dll => 0x1e352a56 => 215
	i32 513247710, ; 80: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 199
	i32 526420162, ; 81: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 82: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 330
	i32 530272170, ; 83: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 84: Microsoft.Extensions.Logging => 0x20216150 => 195
	i32 540030774, ; 85: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 86: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 87: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 548916678, ; 88: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 181
	i32 549171840, ; 89: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 90: Jsr305Binding => 0x213954e7 => 323
	i32 569601784, ; 91: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 321
	i32 577335427, ; 92: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 592146354, ; 93: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 356
	i32 593109234, ; 94: Microsoft.Kiota.Authentication.Azure => 0x235a20f2 => 210
	i32 601371474, ; 95: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 96: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 97: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 618636221, ; 98: K4os.Compression.LZ4.Streams => 0x24dfa3bd => 179
	i32 627609679, ; 99: Xamarin.AndroidX.CustomView => 0x2568904f => 275
	i32 627931235, ; 100: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 354
	i32 639843206, ; 101: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 281
	i32 643868501, ; 102: System.Net => 0x2660a755 => 81
	i32 662205335, ; 103: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 104: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 317
	i32 666292255, ; 105: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 262
	i32 672442732, ; 106: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683423185, ; 107: Microsoft.Kiota.Serialization.Json => 0x28bc35d1 => 213
	i32 683518922, ; 108: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 109: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 336
	i32 690569205, ; 110: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 111: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 332
	i32 693804605, ; 112: System.Windows => 0x295a9e3d => 154
	i32 695450347, ; 113: Syncfusion.Maui.Popup => 0x2973baeb => 241
	i32 699345723, ; 114: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 115: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 327
	i32 700358131, ; 116: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 117: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 351
	i32 709557578, ; 118: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 339
	i32 715014644, ; 119: Pomelo.EntityFrameworkCore.MySql => 0x2a9e41f4 => 224
	i32 720511267, ; 120: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 331
	i32 722857257, ; 121: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 723796036, ; 122: System.ClientModel.dll => 0x2b244044 => 242
	i32 735137430, ; 123: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 739213751, ; 124: Serilog.Settings.Configuration.dll => 0x2c0f81b7 => 227
	i32 748832960, ; 125: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 231
	i32 752232764, ; 126: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 127: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 252
	i32 759454413, ; 128: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 129: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 130: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 131: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 360
	i32 789151979, ; 132: Microsoft.Extensions.Options => 0x2f0980eb => 198
	i32 790371945, ; 133: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 276
	i32 804715423, ; 134: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 135: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 290
	i32 812630446, ; 136: Serilog => 0x306fc1ae => 225
	i32 823281589, ; 137: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 138: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 139: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 140: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 141: Xamarin.AndroidX.Print => 0x3246f6cd => 303
	i32 873119928, ; 142: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 143: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 144: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 145: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 146: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 919194361, ; 147: Syncfusion.Maui.Calendar.dll => 0x36c9caf9 => 239
	i32 926902833, ; 148: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 363
	i32 928116545, ; 149: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 326
	i32 952186615, ; 150: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 956575887, ; 151: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 331
	i32 966729478, ; 152: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 324
	i32 967690846, ; 153: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 287
	i32 975236339, ; 154: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 155: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 983077409, ; 156: MySql.Data.dll => 0x3a989221 => 222
	i32 986514023, ; 157: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 158: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 159: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 160: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 161: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 162: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 307
	i32 1019214401, ; 163: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 164: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 193
	i32 1029334545, ; 165: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 338
	i32 1031528504, ; 166: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 325
	i32 1035644815, ; 167: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 260
	i32 1036536393, ; 168: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 169: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 170: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 294
	i32 1055979864, ; 171: SQLitePCLRaw.lib.e_sqlcipher.android.dll => 0x3ef0f958 => 233
	i32 1067306892, ; 172: GoogleGson => 0x3f9dcf8c => 177
	i32 1075584133, ; 173: SQLitePCLRaw.lib.e_sqlcipher.android => 0x401c1c85 => 233
	i32 1082857460, ; 174: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 175: Xamarin.Kotlin.StdLib => 0x409e66d8 => 328
	i32 1089913930, ; 176: System.Diagnostics.EventLog.dll => 0x40f6c44a => 244
	i32 1098259244, ; 177: System => 0x41761b2c => 164
	i32 1118262833, ; 178: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 351
	i32 1121599056, ; 179: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 293
	i32 1127624469, ; 180: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 197
	i32 1145483052, ; 181: System.Windows.Extensions.dll => 0x4446af2c => 250
	i32 1149092582, ; 182: Xamarin.AndroidX.Window => 0x447dc2e6 => 320
	i32 1157931901, ; 183: Microsoft.EntityFrameworkCore.Abstractions => 0x4504a37d => 184
	i32 1168523401, ; 184: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 357
	i32 1170634674, ; 185: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 186: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 316
	i32 1178241025, ; 187: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 301
	i32 1202000627, ; 188: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x47a512f3 => 184
	i32 1203215381, ; 189: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 355
	i32 1204270330, ; 190: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 262
	i32 1204575371, ; 191: Microsoft.Extensions.Caching.Memory.dll => 0x47cc5c8b => 188
	i32 1208641965, ; 192: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1219128291, ; 193: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1234928153, ; 194: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 353
	i32 1243150071, ; 195: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 321
	i32 1253011324, ; 196: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 197: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 337
	i32 1264511973, ; 198: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 311
	i32 1267360935, ; 199: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 315
	i32 1273260888, ; 200: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 267
	i32 1275534314, ; 201: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 332
	i32 1278448581, ; 202: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 259
	i32 1292207520, ; 203: SQLitePCLRaw.core.dll => 0x4d0585a0 => 232
	i32 1293217323, ; 204: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 278
	i32 1309188875, ; 205: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 206: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 320
	i32 1322857724, ; 207: Serilog.Sinks.File.dll => 0x4ed934fc => 229
	i32 1324164729, ; 208: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 209: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 210: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 211: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 367
	i32 1376866003, ; 212: Xamarin.AndroidX.SavedState => 0x52114ed3 => 307
	i32 1379779777, ; 213: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 214: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 215: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 271
	i32 1408764838, ; 216: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 217: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1418420637, ; 218: Microsoft.Kiota.Authentication.Azure.dll => 0x548b619d => 210
	i32 1422114320, ; 219: Microsoft.Kiota.Abstractions => 0x54c3be10 => 209
	i32 1422545099, ; 220: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 221: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 335
	i32 1434145427, ; 222: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 223: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 324
	i32 1439761251, ; 224: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 225: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 226: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 227: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 228: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1460893475, ; 229: System.IdentityModel.Tokens.Jwt => 0x57137723 => 245
	i32 1461004990, ; 230: es\Microsoft.Maui.Controls.resources => 0x57152abe => 341
	i32 1461234159, ; 231: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 232: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 233: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 234: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 261
	i32 1470490898, ; 235: Microsoft.Extensions.Primitives => 0x57a5e912 => 199
	i32 1479771757, ; 236: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 237: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 238: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1487250139, ; 239: K4os.Hash.xxHash => 0x58a5a2db => 180
	i32 1490025113, ; 240: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 308
	i32 1490351284, ; 241: Microsoft.Data.Sqlite.dll => 0x58d4f4b4 => 182
	i32 1493001747, ; 242: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 345
	i32 1498168481, ; 243: Microsoft.IdentityModel.JsonWebTokens.dll => 0x594c3ca1 => 203
	i32 1511525525, ; 244: MySqlConnector => 0x5a180c95 => 223
	i32 1513823142, ; 245: Serilog.Settings.Configuration => 0x5a3b1ba6 => 227
	i32 1514721132, ; 246: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 340
	i32 1534512859, ; 247: Std.UriTemplate => 0x5b76cedb => 237
	i32 1536373174, ; 248: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 249: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 250: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 251: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 252: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 360
	i32 1564802854, ; 253: SQLitePCLRaw.provider.e_sqlcipher.dll => 0x5d44ff26 => 235
	i32 1565862583, ; 254: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 255: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 256: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 257: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 258: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 277
	i32 1592978981, ; 259: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 260: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 325
	i32 1601112923, ; 261: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 262: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 263: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 264: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 297
	i32 1622358360, ; 265: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 266: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 319
	i32 1625558452, ; 267: Serilog.dll => 0x60e40db4 => 225
	i32 1628113371, ; 268: Microsoft.IdentityModel.Protocols.OpenIdConnect => 0x610b09db => 206
	i32 1635184631, ; 269: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 281
	i32 1636350590, ; 270: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 274
	i32 1639515021, ; 271: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 272: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 273: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 274: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 275: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 313
	i32 1658251792, ; 276: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 322
	i32 1670060433, ; 277: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 269
	i32 1675553242, ; 278: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 279: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 280: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 281: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1688112883, ; 282: Microsoft.Data.Sqlite => 0x649e8ef3 => 182
	i32 1689493916, ; 283: Microsoft.EntityFrameworkCore.dll => 0x64b3a19c => 183
	i32 1691477237, ; 284: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 285: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 286: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 329
	i32 1701541528, ; 287: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1711441057, ; 288: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 234
	i32 1720223769, ; 289: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 290
	i32 1726116996, ; 290: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 291: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 292: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 265
	i32 1736233607, ; 293: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 358
	i32 1743415430, ; 294: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 336
	i32 1744735666, ; 295: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 296: System.IO.Pipelines.dll => 0x68139a0d => 246
	i32 1746316138, ; 297: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 298: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 299: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 300: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 301: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 302: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 312
	i32 1770582343, ; 303: Microsoft.Extensions.Logging.dll => 0x6988f147 => 195
	i32 1776026572, ; 304: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 305: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 306: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 307: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 352
	i32 1788241197, ; 308: Xamarin.AndroidX.Fragment => 0x6a96652d => 283
	i32 1793755602, ; 309: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 344
	i32 1796167890, ; 310: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 181
	i32 1808609942, ; 311: Xamarin.AndroidX.Loader => 0x6bcd3296 => 297
	i32 1813058853, ; 312: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 328
	i32 1813201214, ; 313: Xamarin.Google.Android.Material => 0x6c13413e => 322
	i32 1818569960, ; 314: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 302
	i32 1818787751, ; 315: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 316: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 317: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1828688058, ; 318: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 196
	i32 1829150748, ; 319: System.Windows.Extensions => 0x6d06a01c => 250
	i32 1842015223, ; 320: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 364
	i32 1847515442, ; 321: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 252
	i32 1853025655, ; 322: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 361
	i32 1858542181, ; 323: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 324: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1871986876, ; 325: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 0x6f9440bc => 206
	i32 1875935024, ; 326: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 343
	i32 1879696579, ; 327: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 328: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 263
	i32 1886040351, ; 329: Microsoft.EntityFrameworkCore.Sqlite.dll => 0x706ab11f => 186
	i32 1888955245, ; 330: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 331: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 332: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 333: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 334: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1925302748, ; 335: K4os.Compression.LZ4.dll => 0x72c1c9dc => 178
	i32 1939592360, ; 336: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 337: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 338: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 309
	i32 1968388702, ; 339: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 189
	i32 1983156543, ; 340: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 329
	i32 1985761444, ; 341: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 254
	i32 1986222447, ; 342: Microsoft.IdentityModel.Tokens.dll => 0x7663596f => 207
	i32 2003115576, ; 343: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 340
	i32 2011961780, ; 344: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2014489277, ; 345: Microsoft.EntityFrameworkCore.Sqlite => 0x7812aabd => 186
	i32 2019465201, ; 346: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 294
	i32 2025202353, ; 347: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 335
	i32 2031763787, ; 348: Xamarin.Android.Glide => 0x791a414b => 251
	i32 2045470958, ; 349: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 350: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 191
	i32 2055257422, ; 351: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 289
	i32 2060060697, ; 352: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 353: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 339
	i32 2070888862, ; 354: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 355: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 356: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 357: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 236
	i32 2113869790, ; 358: Microsoft.Graph => 0x7dff17de => 200
	i32 2127167465, ; 359: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 360: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 361: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 362: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 363: Microsoft.Maui => 0x80bd55ad => 219
	i32 2169148018, ; 364: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 347
	i32 2171397733, ; 365: Serilog.Sinks.Console.dll => 0x816ce665 => 228
	i32 2181485124, ; 366: Serilog.Sinks.File => 0x8206d244 => 229
	i32 2181898931, ; 367: Microsoft.Extensions.Options.dll => 0x820d22b3 => 198
	i32 2192057212, ; 368: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 196
	i32 2193016926, ; 369: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2197979891, ; 370: Microsoft.Extensions.DependencyModel.dll => 0x830282f3 => 194
	i32 2201107256, ; 371: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 333
	i32 2201231467, ; 372: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 373: it\Microsoft.Maui.Controls.resources => 0x839595db => 349
	i32 2217644978, ; 374: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 316
	i32 2222056684, ; 375: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 376: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 298
	i32 2252106437, ; 377: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2252897993, ; 378: Microsoft.EntityFrameworkCore => 0x86487ec9 => 183
	i32 2253551641, ; 379: Microsoft.IdentityModel.Protocols => 0x86527819 => 205
	i32 2256313426, ; 380: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 381: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 382: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 190
	i32 2267999099, ; 383: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 253
	i32 2270573516, ; 384: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 343
	i32 2279755925, ; 385: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 305
	i32 2293034957, ; 386: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 387: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 388: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 389: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 353
	i32 2305521784, ; 390: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 391: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 257
	i32 2320631194, ; 392: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 393: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 394: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 395: System.Net.Primitives => 0x8c40e0db => 70
	i32 2354730003, ; 396: Syncfusion.Licensing => 0x8c5a5413 => 238
	i32 2358249420, ; 397: Serilog.Extensions.Logging => 0x8c9007cc => 226
	i32 2368005991, ; 398: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2369706906, ; 399: Microsoft.IdentityModel.Logging => 0x8d3edb9a => 204
	i32 2371007202, ; 400: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 189
	i32 2378619854, ; 401: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 402: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 403: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 348
	i32 2401565422, ; 404: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 405: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 280
	i32 2421380589, ; 406: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 407: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 267
	i32 2427813419, ; 408: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 345
	i32 2435356389, ; 409: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 410: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2454642406, ; 411: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 412: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 413: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465273461, ; 414: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 231
	i32 2465532216, ; 415: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 270
	i32 2471149680, ; 416: Bingie => 0x934ac070 => 0
	i32 2471841756, ; 417: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 418: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 419: Microsoft.Maui.Controls => 0x93dba8a1 => 217
	i32 2483903535, ; 420: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 421: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2486824558, ; 422: K4os.Hash.xxHash.dll => 0x9439ee6e => 180
	i32 2490993605, ; 423: System.AppContext.dll => 0x94798bc5 => 6
	i32 2498657740, ; 424: BouncyCastle.Cryptography.dll => 0x94ee7dcc => 175
	i32 2501346920, ; 425: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 426: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 292
	i32 2509217888, ; 427: System.Diagnostics.EventLog => 0x958fa060 => 244
	i32 2519222276, ; 428: Syncfusion.Maui.Calendar => 0x96284804 => 239
	i32 2522472828, ; 429: Xamarin.Android.Glide.dll => 0x9659e17c => 251
	i32 2538310050, ; 430: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550034082, ; 431: Microsoft.Kiota.Abstractions.dll => 0x97fe6ea2 => 209
	i32 2550873716, ; 432: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 346
	i32 2562349572, ; 433: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 434: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 435: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 293
	i32 2581819634, ; 436: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 315
	i32 2585220780, ; 437: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 438: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 439: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2593496499, ; 440: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 355
	i32 2605712449, ; 441: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 333
	i32 2611359322, ; 442: ZstdSharp.dll => 0x9ba62e5a => 334
	i32 2615233544, ; 443: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 284
	i32 2616218305, ; 444: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 197
	i32 2617129537, ; 445: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 446: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 447: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 274
	i32 2624644809, ; 448: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 279
	i32 2626831493, ; 449: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 350
	i32 2627185994, ; 450: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2627802292, ; 451: Serilog.Extensions.Logging.dll => 0x9ca114b4 => 226
	i32 2628210652, ; 452: System.Memory.Data => 0x9ca74fdc => 247
	i32 2629843544, ; 453: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 454: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 288
	i32 2634653062, ; 455: Microsoft.EntityFrameworkCore.Relational.dll => 0x9d099d86 => 185
	i32 2640290731, ; 456: Microsoft.IdentityModel.Logging.dll => 0x9d5fa3ab => 204
	i32 2640706905, ; 457: Azure.Core => 0x9d65fd59 => 173
	i32 2660759594, ; 458: System.Security.Cryptography.ProtectedData.dll => 0x9e97f82a => 248
	i32 2663391936, ; 459: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 253
	i32 2663698177, ; 460: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 461: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 462: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 463: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 464: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 465: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 466: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 313
	i32 2715334215, ; 467: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 468: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 469: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 470: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 471: Xamarin.AndroidX.Activity => 0xa2e0939b => 255
	i32 2735172069, ; 472: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 473: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 261
	i32 2740948882, ; 474: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 475: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 476: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 356
	i32 2758225723, ; 477: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 218
	i32 2764765095, ; 478: Microsoft.Maui.dll => 0xa4caf7a7 => 219
	i32 2765824710, ; 479: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 480: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 327
	i32 2778768386, ; 481: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 318
	i32 2779977773, ; 482: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 306
	i32 2785988530, ; 483: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 362
	i32 2788224221, ; 484: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 284
	i32 2801831435, ; 485: Microsoft.Maui.Graphics => 0xa7008e0b => 221
	i32 2803228030, ; 486: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806077508, ; 487: Microsoft.Graph.dll => 0xa7415844 => 200
	i32 2806116107, ; 488: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 341
	i32 2810250172, ; 489: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 271
	i32 2819470561, ; 490: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 491: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 492: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 306
	i32 2824502124, ; 493: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2831556043, ; 494: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 354
	i32 2838993487, ; 495: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 295
	i32 2841355853, ; 496: System.Security.Permissions => 0xa95ba64d => 249
	i32 2847789619, ; 497: Microsoft.EntityFrameworkCore.Relational => 0xa9bdd233 => 185
	i32 2849599387, ; 498: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 499: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 318
	i32 2855708567, ; 500: Xamarin.AndroidX.Transition => 0xaa36a797 => 314
	i32 2861098320, ; 501: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 502: Microsoft.Maui.Essentials => 0xaa8a4878 => 220
	i32 2867946736, ; 503: System.Security.Cryptography.ProtectedData => 0xaaf164f0 => 248
	i32 2868557005, ; 504: Syncfusion.Licensing.dll => 0xaafab4cd => 238
	i32 2870099610, ; 505: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 256
	i32 2875164099, ; 506: Jsr305Binding.dll => 0xab5f85c3 => 323
	i32 2875220617, ; 507: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2875299966, ; 508: Microsoft.Kiota.Http.HttpClientLibrary.dll => 0xab61987e => 211
	i32 2884993177, ; 509: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 282
	i32 2887636118, ; 510: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 511: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 512: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 513: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 514: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 515: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2916838712, ; 516: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 319
	i32 2919462931, ; 517: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 518: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 258
	i32 2936416060, ; 519: System.Resources.Reader => 0xaf06273c => 98
	i32 2938903864, ; 520: Microsoft.Kiota.Serialization.Multipart => 0xaf2c1d38 => 214
	i32 2940926066, ; 521: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 522: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2944313911, ; 523: System.Configuration.ConfigurationManager.dll => 0xaf7eaa37 => 243
	i32 2959614098, ; 524: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 525: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2970759306, ; 526: BCrypt.Net-Next.dll => 0xb112308a => 174
	i32 2972252294, ; 527: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 528: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 278
	i32 2987532451, ; 529: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 309
	i32 2996846495, ; 530: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 291
	i32 3012788804, ; 531: System.Configuration.ConfigurationManager => 0xb3938244 => 243
	i32 3016983068, ; 532: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 311
	i32 3023353419, ; 533: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 534: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 286
	i32 3025069135, ; 535: K4os.Compression.LZ4.Streams.dll => 0xb44ee44f => 179
	i32 3033605958, ; 536: System.Memory.Data.dll => 0xb4d12746 => 247
	i32 3038032645, ; 537: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 369
	i32 3056245963, ; 538: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 308
	i32 3057625584, ; 539: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 299
	i32 3059408633, ; 540: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 541: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3069363400, ; 542: Microsoft.Extensions.Caching.Abstractions.dll => 0xb6f2c4c8 => 187
	i32 3072861693, ; 543: Microsoft.Kiota.Serialization.Form.dll => 0xb72825fd => 212
	i32 3075834255, ; 544: System.Threading.Tasks => 0xb755818f => 144
	i32 3077302341, ; 545: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 347
	i32 3084678329, ; 546: Microsoft.IdentityModel.Tokens => 0xb7dc74b9 => 207
	i32 3089219899, ; 547: ZstdSharp => 0xb821c13b => 334
	i32 3090735792, ; 548: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 549: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 550: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 551: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 552: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 553: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 554: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 555: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3147228406, ; 556: Syncfusion.Maui.Core => 0xbb96e4f6 => 240
	i32 3148237826, ; 557: GoogleGson.dll => 0xbba64c02 => 177
	i32 3159123045, ; 558: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 559: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3176689669, ; 560: Microsoft.IdentityModel.Validators.dll => 0xbd587005 => 208
	i32 3178803400, ; 561: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 300
	i32 3192346100, ; 562: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 563: System.Web => 0xbe592c0c => 153
	i32 3195844289, ; 564: Microsoft.Extensions.Caching.Abstractions => 0xbe7cb6c1 => 187
	i32 3204380047, ; 565: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 566: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 567: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 277
	i32 3213246214, ; 568: System.Security.Permissions.dll => 0xbf863f06 => 249
	i32 3220365878, ; 569: System.Threading => 0xbff2e236 => 148
	i32 3222040828, ; 570: Microsoft.IdentityModel.Validators => 0xc00c70fc => 208
	i32 3226221578, ; 571: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 572: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 573: Xamarin.AndroidX.CardView => 0xc235e84d => 265
	i32 3265493905, ; 574: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 575: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 576: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 577: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 578: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3286872994, ; 579: SQLite-net.dll => 0xc3e9b3a2 => 230
	i32 3290767353, ; 580: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 581: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 582: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 583: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 342
	i32 3312457198, ; 584: Microsoft.IdentityModel.JsonWebTokens => 0xc57015ee => 203
	i32 3316684772, ; 585: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 586: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 275
	i32 3317144872, ; 587: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 588: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 263
	i32 3345895724, ; 589: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 304
	i32 3346324047, ; 590: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 301
	i32 3357674450, ; 591: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 359
	i32 3358260929, ; 592: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 593: SQLitePCLRaw.core => 0xc849ca45 => 232
	i32 3362336904, ; 594: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 256
	i32 3362522851, ; 595: Xamarin.AndroidX.Core => 0xc86c06e3 => 272
	i32 3366347497, ; 596: Java.Interop => 0xc8a662e9 => 168
	i32 3367815115, ; 597: Microsoft.Graph.Core.dll => 0xc8bcc7cb => 201
	i32 3374879918, ; 598: Microsoft.IdentityModel.Protocols.dll => 0xc92894ae => 205
	i32 3374999561, ; 599: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 305
	i32 3381016424, ; 600: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 338
	i32 3381033598, ; 601: K4os.Compression.LZ4 => 0xc9867a7e => 178
	i32 3395150330, ; 602: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 603: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 604: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 276
	i32 3421170118, ; 605: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 191
	i32 3428513518, ; 606: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 192
	i32 3429136800, ; 607: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 608: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 609: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 279
	i32 3445260447, ; 610: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 611: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 216
	i32 3454422360, ; 612: Microsoft.Kiota.Serialization.Multipart.dll => 0xcde64d58 => 214
	i32 3463511458, ; 613: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 346
	i32 3467345667, ; 614: MySql.Data => 0xceab7f03 => 222
	i32 3471940407, ; 615: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3472012038, ; 616: BCrypt.Net-Next => 0xcef2b306 => 174
	i32 3476120550, ; 617: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 618: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 359
	i32 3484440000, ; 619: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 358
	i32 3485117614, ; 620: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 621: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 622: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 268
	i32 3499097210, ; 623: Google.Protobuf.dll => 0xd08ffc7a => 176
	i32 3509114376, ; 624: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 625: System.Security.dll => 0xd1854eb4 => 130
	i32 3520934708, ; 626: Microsoft.Kiota.Serialization.Form => 0xd1dd3334 => 212
	i32 3530912306, ; 627: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 628: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3558648585, ; 629: System.ClientModel => 0xd41cab09 => 242
	i32 3560100363, ; 630: System.Threading.Timer => 0xd432d20b => 147
	i32 3561949811, ; 631: Azure.Core.dll => 0xd44f0a73 => 173
	i32 3570554715, ; 632: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 633: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 366
	i32 3597029428, ; 634: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 254
	i32 3598340787, ; 635: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3605570793, ; 636: BouncyCastle.Cryptography => 0xd6e8a4e9 => 175
	i32 3608519521, ; 637: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 638: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 639: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 303
	i32 3633644679, ; 640: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 258
	i32 3638274909, ; 641: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 642: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 289
	i32 3643446276, ; 643: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 363
	i32 3643854240, ; 644: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 300
	i32 3645089577, ; 645: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3645630983, ; 646: Google.Protobuf => 0xd94bea07 => 176
	i32 3657292374, ; 647: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 190
	i32 3657523560, ; 648: Bingie.dll => 0xda016168 => 0
	i32 3660523487, ; 649: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 650: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 651: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 264
	i32 3684561358, ; 652: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 268
	i32 3687915678, ; 653: Microsoft.Kiota.Http.HttpClientLibrary => 0xdbd1209e => 211
	i32 3697841164, ; 654: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 368
	i32 3700591436, ; 655: Microsoft.IdentityModel.Abstractions.dll => 0xdc928b4c => 202
	i32 3700866549, ; 656: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 657: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 273
	i32 3716563718, ; 658: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 659: Xamarin.AndroidX.Annotation => 0xdda814c6 => 257
	i32 3724971120, ; 660: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 299
	i32 3732100267, ; 661: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 662: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 663: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 664: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3754567612, ; 665: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 236
	i32 3786282454, ; 666: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 266
	i32 3792276235, ; 667: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 668: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 216
	i32 3802395368, ; 669: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3819260425, ; 670: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 671: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3826434917, ; 672: Microsoft.Graph.Core => 0xe412c365 => 201
	i32 3829621856, ; 673: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 674: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 193
	i32 3844307129, ; 675: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 676: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 677: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 678: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 679: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 680: SQLite-net => 0xe70c9739 => 230
	i32 3885497537, ; 681: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 682: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 314
	i32 3888767677, ; 683: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 304
	i32 3889960447, ; 684: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 367
	i32 3896106733, ; 685: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 686: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 272
	i32 3901907137, ; 687: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 688: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 689: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 317
	i32 3928044579, ; 690: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 691: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 692: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 302
	i32 3945713374, ; 693: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 694: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 695: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 260
	i32 3959773229, ; 696: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 291
	i32 3980434154, ; 697: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 362
	i32 3987592930, ; 698: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 344
	i32 4003436829, ; 699: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4014412877, ; 700: Microsoft.Kiota.Serialization.Json.dll => 0xef47144d => 213
	i32 4015948917, ; 701: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 259
	i32 4023392905, ; 702: System.IO.Pipelines => 0xefd01a89 => 246
	i32 4025784931, ; 703: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 704: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 218
	i32 4054681211, ; 705: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 706: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 707: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4076551833, ; 708: Std.UriTemplate.dll => 0xf2fb3e99 => 237
	i32 4079385022, ; 709: MySqlConnector.dll => 0xf32679be => 223
	i32 4094352644, ; 710: Microsoft.Maui.Essentials.dll => 0xf40add04 => 220
	i32 4099507663, ; 711: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 712: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 713: Xamarin.AndroidX.Emoji2 => 0xf479582c => 280
	i32 4101842092, ; 714: Microsoft.Extensions.Caching.Memory => 0xf47d24ac => 188
	i32 4102112229, ; 715: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 357
	i32 4125707920, ; 716: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 352
	i32 4126470640, ; 717: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 192
	i32 4127667938, ; 718: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 719: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 720: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 721: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 364
	i32 4151237749, ; 722: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 723: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 724: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 725: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 726: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 727: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 296
	i32 4185676441, ; 728: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 729: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 730: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4254412227, ; 731: SQLitePCLRaw.provider.e_sqlcipher => 0xfd952dc3 => 235
	i32 4256097574, ; 732: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 273
	i32 4258378803, ; 733: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 295
	i32 4260525087, ; 734: System.Buffers => 0xfdf2741f => 7
	i32 4263231520, ; 735: System.IdentityModel.Tokens.Jwt.dll => 0xfe1bc020 => 245
	i32 4271975918, ; 736: Microsoft.Maui.Controls.dll => 0xfea12dee => 217
	i32 4274976490, ; 737: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 738: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 296
	i32 4294763496 ; 739: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 282
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [740 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 194, ; 3
	i32 292, ; 4
	i32 326, ; 5
	i32 48, ; 6
	i32 80, ; 7
	i32 145, ; 8
	i32 30, ; 9
	i32 368, ; 10
	i32 124, ; 11
	i32 221, ; 12
	i32 102, ; 13
	i32 310, ; 14
	i32 107, ; 15
	i32 310, ; 16
	i32 139, ; 17
	i32 330, ; 18
	i32 224, ; 19
	i32 77, ; 20
	i32 124, ; 21
	i32 13, ; 22
	i32 266, ; 23
	i32 240, ; 24
	i32 132, ; 25
	i32 312, ; 26
	i32 151, ; 27
	i32 215, ; 28
	i32 365, ; 29
	i32 366, ; 30
	i32 18, ; 31
	i32 264, ; 32
	i32 26, ; 33
	i32 286, ; 34
	i32 1, ; 35
	i32 59, ; 36
	i32 42, ; 37
	i32 91, ; 38
	i32 269, ; 39
	i32 147, ; 40
	i32 288, ; 41
	i32 285, ; 42
	i32 337, ; 43
	i32 54, ; 44
	i32 69, ; 45
	i32 365, ; 46
	i32 255, ; 47
	i32 83, ; 48
	i32 350, ; 49
	i32 287, ; 50
	i32 234, ; 51
	i32 349, ; 52
	i32 131, ; 53
	i32 55, ; 54
	i32 241, ; 55
	i32 149, ; 56
	i32 74, ; 57
	i32 145, ; 58
	i32 62, ; 59
	i32 146, ; 60
	i32 369, ; 61
	i32 228, ; 62
	i32 165, ; 63
	i32 361, ; 64
	i32 270, ; 65
	i32 12, ; 66
	i32 283, ; 67
	i32 125, ; 68
	i32 152, ; 69
	i32 113, ; 70
	i32 166, ; 71
	i32 164, ; 72
	i32 285, ; 73
	i32 202, ; 74
	i32 298, ; 75
	i32 84, ; 76
	i32 348, ; 77
	i32 342, ; 78
	i32 215, ; 79
	i32 199, ; 80
	i32 150, ; 81
	i32 330, ; 82
	i32 60, ; 83
	i32 195, ; 84
	i32 51, ; 85
	i32 103, ; 86
	i32 114, ; 87
	i32 181, ; 88
	i32 40, ; 89
	i32 323, ; 90
	i32 321, ; 91
	i32 120, ; 92
	i32 356, ; 93
	i32 210, ; 94
	i32 52, ; 95
	i32 44, ; 96
	i32 119, ; 97
	i32 179, ; 98
	i32 275, ; 99
	i32 354, ; 100
	i32 281, ; 101
	i32 81, ; 102
	i32 136, ; 103
	i32 317, ; 104
	i32 262, ; 105
	i32 8, ; 106
	i32 213, ; 107
	i32 73, ; 108
	i32 336, ; 109
	i32 155, ; 110
	i32 332, ; 111
	i32 154, ; 112
	i32 241, ; 113
	i32 92, ; 114
	i32 327, ; 115
	i32 45, ; 116
	i32 351, ; 117
	i32 339, ; 118
	i32 224, ; 119
	i32 331, ; 120
	i32 109, ; 121
	i32 242, ; 122
	i32 129, ; 123
	i32 227, ; 124
	i32 231, ; 125
	i32 25, ; 126
	i32 252, ; 127
	i32 72, ; 128
	i32 55, ; 129
	i32 46, ; 130
	i32 360, ; 131
	i32 198, ; 132
	i32 276, ; 133
	i32 22, ; 134
	i32 290, ; 135
	i32 225, ; 136
	i32 86, ; 137
	i32 43, ; 138
	i32 160, ; 139
	i32 71, ; 140
	i32 303, ; 141
	i32 3, ; 142
	i32 42, ; 143
	i32 63, ; 144
	i32 16, ; 145
	i32 53, ; 146
	i32 239, ; 147
	i32 363, ; 148
	i32 326, ; 149
	i32 105, ; 150
	i32 331, ; 151
	i32 324, ; 152
	i32 287, ; 153
	i32 34, ; 154
	i32 158, ; 155
	i32 222, ; 156
	i32 85, ; 157
	i32 32, ; 158
	i32 12, ; 159
	i32 51, ; 160
	i32 56, ; 161
	i32 307, ; 162
	i32 36, ; 163
	i32 193, ; 164
	i32 338, ; 165
	i32 325, ; 166
	i32 260, ; 167
	i32 35, ; 168
	i32 58, ; 169
	i32 294, ; 170
	i32 233, ; 171
	i32 177, ; 172
	i32 233, ; 173
	i32 17, ; 174
	i32 328, ; 175
	i32 244, ; 176
	i32 164, ; 177
	i32 351, ; 178
	i32 293, ; 179
	i32 197, ; 180
	i32 250, ; 181
	i32 320, ; 182
	i32 184, ; 183
	i32 357, ; 184
	i32 153, ; 185
	i32 316, ; 186
	i32 301, ; 187
	i32 184, ; 188
	i32 355, ; 189
	i32 262, ; 190
	i32 188, ; 191
	i32 29, ; 192
	i32 52, ; 193
	i32 353, ; 194
	i32 321, ; 195
	i32 5, ; 196
	i32 337, ; 197
	i32 311, ; 198
	i32 315, ; 199
	i32 267, ; 200
	i32 332, ; 201
	i32 259, ; 202
	i32 232, ; 203
	i32 278, ; 204
	i32 85, ; 205
	i32 320, ; 206
	i32 229, ; 207
	i32 61, ; 208
	i32 112, ; 209
	i32 57, ; 210
	i32 367, ; 211
	i32 307, ; 212
	i32 99, ; 213
	i32 19, ; 214
	i32 271, ; 215
	i32 111, ; 216
	i32 101, ; 217
	i32 210, ; 218
	i32 209, ; 219
	i32 102, ; 220
	i32 335, ; 221
	i32 104, ; 222
	i32 324, ; 223
	i32 71, ; 224
	i32 38, ; 225
	i32 32, ; 226
	i32 103, ; 227
	i32 73, ; 228
	i32 245, ; 229
	i32 341, ; 230
	i32 9, ; 231
	i32 123, ; 232
	i32 46, ; 233
	i32 261, ; 234
	i32 199, ; 235
	i32 9, ; 236
	i32 43, ; 237
	i32 4, ; 238
	i32 180, ; 239
	i32 308, ; 240
	i32 182, ; 241
	i32 345, ; 242
	i32 203, ; 243
	i32 223, ; 244
	i32 227, ; 245
	i32 340, ; 246
	i32 237, ; 247
	i32 31, ; 248
	i32 138, ; 249
	i32 92, ; 250
	i32 93, ; 251
	i32 360, ; 252
	i32 235, ; 253
	i32 49, ; 254
	i32 141, ; 255
	i32 112, ; 256
	i32 140, ; 257
	i32 277, ; 258
	i32 115, ; 259
	i32 325, ; 260
	i32 157, ; 261
	i32 76, ; 262
	i32 79, ; 263
	i32 297, ; 264
	i32 37, ; 265
	i32 319, ; 266
	i32 225, ; 267
	i32 206, ; 268
	i32 281, ; 269
	i32 274, ; 270
	i32 64, ; 271
	i32 138, ; 272
	i32 15, ; 273
	i32 116, ; 274
	i32 313, ; 275
	i32 322, ; 276
	i32 269, ; 277
	i32 48, ; 278
	i32 70, ; 279
	i32 80, ; 280
	i32 126, ; 281
	i32 182, ; 282
	i32 183, ; 283
	i32 94, ; 284
	i32 121, ; 285
	i32 329, ; 286
	i32 26, ; 287
	i32 234, ; 288
	i32 290, ; 289
	i32 97, ; 290
	i32 28, ; 291
	i32 265, ; 292
	i32 358, ; 293
	i32 336, ; 294
	i32 149, ; 295
	i32 246, ; 296
	i32 169, ; 297
	i32 4, ; 298
	i32 98, ; 299
	i32 33, ; 300
	i32 93, ; 301
	i32 312, ; 302
	i32 195, ; 303
	i32 21, ; 304
	i32 41, ; 305
	i32 170, ; 306
	i32 352, ; 307
	i32 283, ; 308
	i32 344, ; 309
	i32 181, ; 310
	i32 297, ; 311
	i32 328, ; 312
	i32 322, ; 313
	i32 302, ; 314
	i32 2, ; 315
	i32 134, ; 316
	i32 111, ; 317
	i32 196, ; 318
	i32 250, ; 319
	i32 364, ; 320
	i32 252, ; 321
	i32 361, ; 322
	i32 58, ; 323
	i32 95, ; 324
	i32 206, ; 325
	i32 343, ; 326
	i32 39, ; 327
	i32 263, ; 328
	i32 186, ; 329
	i32 25, ; 330
	i32 94, ; 331
	i32 89, ; 332
	i32 99, ; 333
	i32 10, ; 334
	i32 178, ; 335
	i32 87, ; 336
	i32 100, ; 337
	i32 309, ; 338
	i32 189, ; 339
	i32 329, ; 340
	i32 254, ; 341
	i32 207, ; 342
	i32 340, ; 343
	i32 7, ; 344
	i32 186, ; 345
	i32 294, ; 346
	i32 335, ; 347
	i32 251, ; 348
	i32 88, ; 349
	i32 191, ; 350
	i32 289, ; 351
	i32 154, ; 352
	i32 339, ; 353
	i32 33, ; 354
	i32 116, ; 355
	i32 82, ; 356
	i32 236, ; 357
	i32 200, ; 358
	i32 20, ; 359
	i32 11, ; 360
	i32 162, ; 361
	i32 3, ; 362
	i32 219, ; 363
	i32 347, ; 364
	i32 228, ; 365
	i32 229, ; 366
	i32 198, ; 367
	i32 196, ; 368
	i32 84, ; 369
	i32 194, ; 370
	i32 333, ; 371
	i32 64, ; 372
	i32 349, ; 373
	i32 316, ; 374
	i32 143, ; 375
	i32 298, ; 376
	i32 157, ; 377
	i32 183, ; 378
	i32 205, ; 379
	i32 41, ; 380
	i32 117, ; 381
	i32 190, ; 382
	i32 253, ; 383
	i32 343, ; 384
	i32 305, ; 385
	i32 131, ; 386
	i32 75, ; 387
	i32 66, ; 388
	i32 353, ; 389
	i32 172, ; 390
	i32 257, ; 391
	i32 143, ; 392
	i32 106, ; 393
	i32 151, ; 394
	i32 70, ; 395
	i32 238, ; 396
	i32 226, ; 397
	i32 156, ; 398
	i32 204, ; 399
	i32 189, ; 400
	i32 121, ; 401
	i32 127, ; 402
	i32 348, ; 403
	i32 152, ; 404
	i32 280, ; 405
	i32 141, ; 406
	i32 267, ; 407
	i32 345, ; 408
	i32 20, ; 409
	i32 14, ; 410
	i32 135, ; 411
	i32 75, ; 412
	i32 59, ; 413
	i32 231, ; 414
	i32 270, ; 415
	i32 0, ; 416
	i32 167, ; 417
	i32 168, ; 418
	i32 217, ; 419
	i32 15, ; 420
	i32 74, ; 421
	i32 180, ; 422
	i32 6, ; 423
	i32 175, ; 424
	i32 23, ; 425
	i32 292, ; 426
	i32 244, ; 427
	i32 239, ; 428
	i32 251, ; 429
	i32 91, ; 430
	i32 209, ; 431
	i32 346, ; 432
	i32 1, ; 433
	i32 136, ; 434
	i32 293, ; 435
	i32 315, ; 436
	i32 134, ; 437
	i32 69, ; 438
	i32 146, ; 439
	i32 355, ; 440
	i32 333, ; 441
	i32 334, ; 442
	i32 284, ; 443
	i32 197, ; 444
	i32 88, ; 445
	i32 96, ; 446
	i32 274, ; 447
	i32 279, ; 448
	i32 350, ; 449
	i32 31, ; 450
	i32 226, ; 451
	i32 247, ; 452
	i32 45, ; 453
	i32 288, ; 454
	i32 185, ; 455
	i32 204, ; 456
	i32 173, ; 457
	i32 248, ; 458
	i32 253, ; 459
	i32 109, ; 460
	i32 158, ; 461
	i32 35, ; 462
	i32 22, ; 463
	i32 114, ; 464
	i32 57, ; 465
	i32 313, ; 466
	i32 144, ; 467
	i32 118, ; 468
	i32 120, ; 469
	i32 110, ; 470
	i32 255, ; 471
	i32 139, ; 472
	i32 261, ; 473
	i32 54, ; 474
	i32 105, ; 475
	i32 356, ; 476
	i32 218, ; 477
	i32 219, ; 478
	i32 133, ; 479
	i32 327, ; 480
	i32 318, ; 481
	i32 306, ; 482
	i32 362, ; 483
	i32 284, ; 484
	i32 221, ; 485
	i32 159, ; 486
	i32 200, ; 487
	i32 341, ; 488
	i32 271, ; 489
	i32 163, ; 490
	i32 132, ; 491
	i32 306, ; 492
	i32 161, ; 493
	i32 354, ; 494
	i32 295, ; 495
	i32 249, ; 496
	i32 185, ; 497
	i32 140, ; 498
	i32 318, ; 499
	i32 314, ; 500
	i32 169, ; 501
	i32 220, ; 502
	i32 248, ; 503
	i32 238, ; 504
	i32 256, ; 505
	i32 323, ; 506
	i32 40, ; 507
	i32 211, ; 508
	i32 282, ; 509
	i32 81, ; 510
	i32 56, ; 511
	i32 37, ; 512
	i32 97, ; 513
	i32 166, ; 514
	i32 172, ; 515
	i32 319, ; 516
	i32 82, ; 517
	i32 258, ; 518
	i32 98, ; 519
	i32 214, ; 520
	i32 30, ; 521
	i32 159, ; 522
	i32 243, ; 523
	i32 18, ; 524
	i32 127, ; 525
	i32 174, ; 526
	i32 119, ; 527
	i32 278, ; 528
	i32 309, ; 529
	i32 291, ; 530
	i32 243, ; 531
	i32 311, ; 532
	i32 165, ; 533
	i32 286, ; 534
	i32 179, ; 535
	i32 247, ; 536
	i32 369, ; 537
	i32 308, ; 538
	i32 299, ; 539
	i32 170, ; 540
	i32 16, ; 541
	i32 187, ; 542
	i32 212, ; 543
	i32 144, ; 544
	i32 347, ; 545
	i32 207, ; 546
	i32 334, ; 547
	i32 125, ; 548
	i32 118, ; 549
	i32 38, ; 550
	i32 115, ; 551
	i32 47, ; 552
	i32 142, ; 553
	i32 117, ; 554
	i32 34, ; 555
	i32 240, ; 556
	i32 177, ; 557
	i32 95, ; 558
	i32 53, ; 559
	i32 208, ; 560
	i32 300, ; 561
	i32 129, ; 562
	i32 153, ; 563
	i32 187, ; 564
	i32 24, ; 565
	i32 161, ; 566
	i32 277, ; 567
	i32 249, ; 568
	i32 148, ; 569
	i32 208, ; 570
	i32 104, ; 571
	i32 89, ; 572
	i32 265, ; 573
	i32 60, ; 574
	i32 142, ; 575
	i32 100, ; 576
	i32 5, ; 577
	i32 13, ; 578
	i32 230, ; 579
	i32 122, ; 580
	i32 135, ; 581
	i32 28, ; 582
	i32 342, ; 583
	i32 203, ; 584
	i32 72, ; 585
	i32 275, ; 586
	i32 24, ; 587
	i32 263, ; 588
	i32 304, ; 589
	i32 301, ; 590
	i32 359, ; 591
	i32 137, ; 592
	i32 232, ; 593
	i32 256, ; 594
	i32 272, ; 595
	i32 168, ; 596
	i32 201, ; 597
	i32 205, ; 598
	i32 305, ; 599
	i32 338, ; 600
	i32 178, ; 601
	i32 101, ; 602
	i32 123, ; 603
	i32 276, ; 604
	i32 191, ; 605
	i32 192, ; 606
	i32 163, ; 607
	i32 167, ; 608
	i32 279, ; 609
	i32 39, ; 610
	i32 216, ; 611
	i32 214, ; 612
	i32 346, ; 613
	i32 222, ; 614
	i32 17, ; 615
	i32 174, ; 616
	i32 171, ; 617
	i32 359, ; 618
	i32 358, ; 619
	i32 137, ; 620
	i32 150, ; 621
	i32 268, ; 622
	i32 176, ; 623
	i32 155, ; 624
	i32 130, ; 625
	i32 212, ; 626
	i32 19, ; 627
	i32 65, ; 628
	i32 242, ; 629
	i32 147, ; 630
	i32 173, ; 631
	i32 47, ; 632
	i32 366, ; 633
	i32 254, ; 634
	i32 79, ; 635
	i32 175, ; 636
	i32 61, ; 637
	i32 106, ; 638
	i32 303, ; 639
	i32 258, ; 640
	i32 49, ; 641
	i32 289, ; 642
	i32 363, ; 643
	i32 300, ; 644
	i32 14, ; 645
	i32 176, ; 646
	i32 190, ; 647
	i32 0, ; 648
	i32 68, ; 649
	i32 171, ; 650
	i32 264, ; 651
	i32 268, ; 652
	i32 211, ; 653
	i32 368, ; 654
	i32 202, ; 655
	i32 78, ; 656
	i32 273, ; 657
	i32 108, ; 658
	i32 257, ; 659
	i32 299, ; 660
	i32 67, ; 661
	i32 63, ; 662
	i32 27, ; 663
	i32 160, ; 664
	i32 236, ; 665
	i32 266, ; 666
	i32 10, ; 667
	i32 216, ; 668
	i32 11, ; 669
	i32 78, ; 670
	i32 126, ; 671
	i32 201, ; 672
	i32 83, ; 673
	i32 193, ; 674
	i32 66, ; 675
	i32 107, ; 676
	i32 65, ; 677
	i32 128, ; 678
	i32 122, ; 679
	i32 230, ; 680
	i32 77, ; 681
	i32 314, ; 682
	i32 304, ; 683
	i32 367, ; 684
	i32 8, ; 685
	i32 272, ; 686
	i32 2, ; 687
	i32 44, ; 688
	i32 317, ; 689
	i32 156, ; 690
	i32 128, ; 691
	i32 302, ; 692
	i32 23, ; 693
	i32 133, ; 694
	i32 260, ; 695
	i32 291, ; 696
	i32 362, ; 697
	i32 344, ; 698
	i32 29, ; 699
	i32 213, ; 700
	i32 259, ; 701
	i32 246, ; 702
	i32 62, ; 703
	i32 218, ; 704
	i32 90, ; 705
	i32 87, ; 706
	i32 148, ; 707
	i32 237, ; 708
	i32 223, ; 709
	i32 220, ; 710
	i32 36, ; 711
	i32 86, ; 712
	i32 280, ; 713
	i32 188, ; 714
	i32 357, ; 715
	i32 352, ; 716
	i32 192, ; 717
	i32 50, ; 718
	i32 6, ; 719
	i32 90, ; 720
	i32 364, ; 721
	i32 21, ; 722
	i32 162, ; 723
	i32 96, ; 724
	i32 50, ; 725
	i32 113, ; 726
	i32 296, ; 727
	i32 130, ; 728
	i32 76, ; 729
	i32 27, ; 730
	i32 235, ; 731
	i32 273, ; 732
	i32 295, ; 733
	i32 7, ; 734
	i32 245, ; 735
	i32 217, ; 736
	i32 110, ; 737
	i32 296, ; 738
	i32 282 ; 739
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
