; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [442 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [878 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 15802525, ; 3: Microsoft.CodeAnalysis.VisualBasic.dll => 0xf1209d => 188
	i32 26230656, ; 4: Microsoft.Extensions.DependencyModel => 0x1903f80 => 204
	i32 27148744, ; 5: Roslyn.VisualStudio.Services.UnitTests.dll => 0x19e41c8 => 235
	i32 27499509, ; 6: de/Microsoft.CodeAnalysis.resources.dll => 0x1a39bf5 => 353
	i32 32687329, ; 7: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 309
	i32 34715100, ; 8: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 343
	i32 34839235, ; 9: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 38835991, ; 10: zh-Hant\Microsoft.CodeAnalysis.Workspaces.resources => 0x2509717 => 403
	i32 39485524, ; 11: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 12: System.Threading.Thread => 0x28aa24d => 145
	i32 48239899, ; 13: de\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x2e0151b => 379
	i32 51709346, ; 14: Roslynator.Interfaces.dll => 0x31505a2 => 236
	i32 66541672, ; 15: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 16: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 437
	i32 68219467, ; 17: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 18: Microsoft.Maui.Graphics.dll => 0x44bb714 => 231
	i32 72810961, ; 19: Microsoft.CodeAnalysis.CSharp.Workspaces => 0x45701d1 => 186
	i32 82292897, ; 20: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 84332821, ; 21: zh-Hans/Microsoft.CodeAnalysis.resources.dll => 0x506d115 => 363
	i32 101534019, ; 22: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 327
	i32 117431740, ; 23: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 24: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 327
	i32 122350210, ; 25: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 26: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 347
	i32 140305987, ; 27: Pomelo.EntityFrameworkCore.MySql.dll => 0x85ce643 => 234
	i32 142721839, ; 28: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 29: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 30: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 31: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 283
	i32 172961045, ; 32: Syncfusion.Maui.Core.dll => 0xa4f2d15 => 252
	i32 176265551, ; 33: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 34: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 329
	i32 184328833, ; 35: System.ValueTuple.dll => 0xafca281 => 151
	i32 189295616, ; 36: Microsoft.Kiota.Serialization.Text => 0xb486c00 => 225
	i32 195452805, ; 37: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 434
	i32 199333315, ; 38: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 435
	i32 205061960, ; 39: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 40: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 281
	i32 220171995, ; 41: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 226563664, ; 42: Roslyn.VisualStudio.Services.UnitTests => 0xd811650 => 235
	i32 230216969, ; 43: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 303
	i32 230752869, ; 44: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 45: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 46: System.Globalization => 0xdd133ce => 42
	i32 233962274, ; 47: zh-Hans\Microsoft.CodeAnalysis.CSharp.resources => 0xdf1fb22 => 376
	i32 246610117, ; 48: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 249310746, ; 49: cs\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xedc2e1a => 378
	i32 255032849, ; 50: Microsoft.CodeAnalysis.CSharp.Features.dll => 0xf337e11 => 185
	i32 261689757, ; 51: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 286
	i32 275679612, ; 52: Humanizer => 0x106e897c => 178
	i32 276479776, ; 53: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 54: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 305
	i32 280482487, ; 55: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 302
	i32 280992041, ; 56: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 406
	i32 291076382, ; 57: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 292653774, ; 58: fr/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x11718ace => 368
	i32 298918909, ; 59: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 60: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 434
	i32 318968648, ; 61: Xamarin.AndroidX.Activity.dll => 0x13031348 => 272
	i32 321597661, ; 62: System.Numerics => 0x132b30dd => 83
	i32 324829778, ; 63: pt-BR\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x135c8252 => 386
	i32 336156722, ; 64: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 419
	i32 342366114, ; 65: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 304
	i32 347068432, ; 66: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 246
	i32 356389973, ; 67: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 418
	i32 360082299, ; 68: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 69: System.IO.Pipes => 0x15ebe147 => 55
	i32 374376850, ; 70: Syncfusion.Maui.Popup.dll => 0x16508992 => 253
	i32 374914964, ; 71: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 72: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 73: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385310584, ; 74: cs/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x16f75f78 => 391
	i32 385762202, ; 75: System.Memory.dll => 0x16fe439a => 62
	i32 386288746, ; 76: System.Composition.Convention.dll => 0x17064c6a => 256
	i32 392610295, ; 77: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 78: _Microsoft.Android.Resource.Designer => 0x17969339 => 438
	i32 398680804, ; 79: Serilog.Sinks.Console => 0x17c362e4 => 240
	i32 403441872, ; 80: WindowsBase => 0x180c08d0 => 165
	i32 403654887, ; 81: zh-Hans/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x180f48e7 => 376
	i32 407564895, ; 82: zh-Hant\Microsoft.CodeAnalysis.resources => 0x184af25f => 364
	i32 429918412, ; 83: tr/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x19a008cc => 388
	i32 435591531, ; 84: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 430
	i32 441335492, ; 85: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 287
	i32 442565967, ; 86: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 87: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 300
	i32 451504562, ; 88: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 89: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 90: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 91: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 92: System.dll => 0x1bff388e => 164
	i32 476646585, ; 93: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 302
	i32 485463106, ; 94: Microsoft.IdentityModel.Abstractions => 0x1cef9442 => 212
	i32 486930444, ; 95: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 315
	i32 498788369, ; 96: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 97: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 417
	i32 503918385, ; 98: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 411
	i32 506800726, ; 99: Microsoft.Kiota.Serialization.Text.dll => 0x1e352a56 => 225
	i32 513247710, ; 100: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 209
	i32 523572874, ; 101: zh-Hant/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x1f35168a => 390
	i32 526420162, ; 102: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 103: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 347
	i32 530272170, ; 104: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 537295123, ; 105: Microsoft.CodeAnalysis.VisualBasic.Workspaces.dll => 0x20067913 => 190
	i32 539058512, ; 106: Microsoft.Extensions.Logging => 0x20216150 => 205
	i32 540030774, ; 107: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545011643, ; 108: it/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x207c37bb => 382
	i32 545304856, ; 109: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 110: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 548916678, ; 111: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 182
	i32 549171840, ; 112: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 113: Jsr305Binding => 0x213954e7 => 340
	i32 569601784, ; 114: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 338
	i32 577335427, ; 115: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 592146354, ; 116: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 425
	i32 593109234, ; 117: Microsoft.Kiota.Authentication.Azure => 0x235a20f2 => 220
	i32 601371474, ; 118: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 119: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 120: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 618636221, ; 121: K4os.Compression.LZ4.Streams => 0x24dfa3bd => 180
	i32 627609679, ; 122: Xamarin.AndroidX.CustomView => 0x2568904f => 292
	i32 627931235, ; 123: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 423
	i32 639843206, ; 124: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 298
	i32 643868501, ; 125: System.Net => 0x2660a755 => 81
	i32 662205335, ; 126: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 127: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 334
	i32 666292255, ; 128: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 279
	i32 672442732, ; 129: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683423185, ; 130: Microsoft.Kiota.Serialization.Json => 0x28bc35d1 => 223
	i32 683518922, ; 131: System.Net.Security => 0x28bdabca => 73
	i32 687422377, ; 132: System.Composition.Convention => 0x28f93ba9 => 256
	i32 688181140, ; 133: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 405
	i32 690569205, ; 134: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 135: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 349
	i32 693804605, ; 136: System.Windows => 0x295a9e3d => 154
	i32 695450347, ; 137: Syncfusion.Maui.Popup => 0x2973baeb => 253
	i32 699345723, ; 138: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 139: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 344
	i32 700358131, ; 140: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 704132156, ; 141: pl/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x29f8343c => 385
	i32 706645707, ; 142: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 420
	i32 709557578, ; 143: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 408
	i32 715014644, ; 144: Pomelo.EntityFrameworkCore.MySql => 0x2a9e41f4 => 234
	i32 720511267, ; 145: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 348
	i32 722857257, ; 146: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 723796036, ; 147: System.ClientModel.dll => 0x2b244044 => 254
	i32 730720346, ; 148: ja/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x2b8de85a => 396
	i32 735137430, ; 149: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 739213751, ; 150: Serilog.Settings.Configuration.dll => 0x2c0f81b7 => 239
	i32 748832960, ; 151: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 243
	i32 752232764, ; 152: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 153: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 269
	i32 759454413, ; 154: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 155: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 156: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 157: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 429
	i32 783888833, ; 158: System.Composition.Runtime.dll => 0x2eb931c1 => 258
	i32 789151979, ; 159: Microsoft.Extensions.Options => 0x2f0980eb => 208
	i32 790371945, ; 160: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 293
	i32 794885870, ; 161: es\Microsoft.CodeAnalysis.resources => 0x2f60feee => 354
	i32 804715423, ; 162: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 163: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 307
	i32 812630446, ; 164: Serilog => 0x306fc1ae => 237
	i32 823281589, ; 165: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 166: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 167: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 168: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 169: Xamarin.AndroidX.Print => 0x3246f6cd => 320
	i32 873119928, ; 170: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 171: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 172: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 173: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 907727466, ; 174: Microsoft.CodeAnalysis.VisualBasic.Workspaces => 0x361ad26a => 190
	i32 911108515, ; 175: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 919194361, ; 176: Syncfusion.Maui.Calendar.dll => 0x36c9caf9 => 251
	i32 926902833, ; 177: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 432
	i32 928116545, ; 178: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 343
	i32 951539854, ; 179: ru\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x38b7588e => 387
	i32 952186615, ; 180: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 956215521, ; 181: fr\Microsoft.CodeAnalysis.CSharp.resources => 0x38feb0e1 => 368
	i32 956575887, ; 182: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 348
	i32 966729478, ; 183: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 341
	i32 967690846, ; 184: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 304
	i32 975236339, ; 185: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 186: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 983077409, ; 187: MySql.Data.dll => 0x3a989221 => 232
	i32 986514023, ; 188: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 189: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 190: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 191: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 192: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 193: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 324
	i32 1019214401, ; 194: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 195: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 203
	i32 1029334545, ; 196: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 407
	i32 1031528504, ; 197: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 342
	i32 1035644815, ; 198: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 277
	i32 1036536393, ; 199: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 200: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 201: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 311
	i32 1055979864, ; 202: SQLitePCLRaw.lib.e_sqlcipher.android.dll => 0x3ef0f958 => 245
	i32 1061829530, ; 203: ru/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x3f4a3b9a => 387
	i32 1067306892, ; 204: GoogleGson => 0x3f9dcf8c => 177
	i32 1075584133, ; 205: SQLitePCLRaw.lib.e_sqlcipher.android => 0x401c1c85 => 245
	i32 1082857460, ; 206: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 207: Xamarin.Kotlin.StdLib => 0x409e66d8 => 345
	i32 1088213606, ; 208: pl\Microsoft.CodeAnalysis.CSharp.resources => 0x40dcd266 => 372
	i32 1089913930, ; 209: System.Diagnostics.EventLog.dll => 0x40f6c44a => 261
	i32 1098259244, ; 210: System => 0x41761b2c => 164
	i32 1098418679, ; 211: ja\Microsoft.CodeAnalysis.CSharp.resources => 0x417889f7 => 370
	i32 1118262833, ; 212: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 420
	i32 1121599056, ; 213: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 310
	i32 1127624469, ; 214: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 207
	i32 1145483052, ; 215: System.Windows.Extensions.dll => 0x4446af2c => 267
	i32 1149092582, ; 216: Xamarin.AndroidX.Window => 0x447dc2e6 => 337
	i32 1157931901, ; 217: Microsoft.EntityFrameworkCore.Abstractions => 0x4504a37d => 194
	i32 1166585964, ; 218: zh-Hant/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x4588b06c => 403
	i32 1168523401, ; 219: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 426
	i32 1170634674, ; 220: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 221: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 333
	i32 1176596968, ; 222: pl/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x462171e8 => 372
	i32 1178241025, ; 223: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 318
	i32 1201690614, ; 224: Microsoft.CodeAnalysis.Features.dll => 0x47a057f6 => 187
	i32 1202000627, ; 225: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x47a512f3 => 194
	i32 1203215381, ; 226: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 424
	i32 1204270330, ; 227: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 279
	i32 1204575371, ; 228: Microsoft.Extensions.Caching.Memory.dll => 0x47cc5c8b => 198
	i32 1208641965, ; 229: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1219128291, ; 230: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1231801191, ; 231: de/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x496bcb67 => 392
	i32 1234928153, ; 232: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 422
	i32 1243150071, ; 233: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 338
	i32 1253011324, ; 234: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 235: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 406
	i32 1264511973, ; 236: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 328
	i32 1267360935, ; 237: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 332
	i32 1273260888, ; 238: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 284
	i32 1275534314, ; 239: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 349
	i32 1278448581, ; 240: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 276
	i32 1292207520, ; 241: SQLitePCLRaw.core.dll => 0x4d0585a0 => 244
	i32 1293217323, ; 242: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 295
	i32 1303681164, ; 243: ja/Microsoft.CodeAnalysis.resources.dll => 0x4db4988c => 357
	i32 1309188875, ; 244: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1317411180, ; 245: fr/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x4e86196c => 394
	i32 1322716291, ; 246: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 337
	i32 1322857724, ; 247: Serilog.Sinks.File.dll => 0x4ed934fc => 241
	i32 1324164729, ; 248: System.Linq => 0x4eed2679 => 61
	i32 1332737275, ; 249: zh-Hant\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x4f6ff4fb => 390
	i32 1333472706, ; 250: zh-Hans/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x4f7b2dc2 => 402
	i32 1335115385, ; 251: cs\Microsoft.CodeAnalysis.Workspaces.resources => 0x4f943e79 => 391
	i32 1335329327, ; 252: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1355941824, ; 253: ja\Microsoft.CodeAnalysis.resources => 0x50d207c0 => 357
	i32 1364015309, ; 254: System.IO => 0x514d38cd => 57
	i32 1367755087, ; 255: ru/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x5186494f => 374
	i32 1373134921, ; 256: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 436
	i32 1376829469, ; 257: cs\Microsoft.CodeAnalysis.CSharp.resources => 0x5210c01d => 365
	i32 1376866003, ; 258: Xamarin.AndroidX.SavedState => 0x52114ed3 => 324
	i32 1379779777, ; 259: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 260: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 261: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 288
	i32 1408764838, ; 262: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 263: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1418420637, ; 264: Microsoft.Kiota.Authentication.Azure.dll => 0x548b619d => 220
	i32 1422114320, ; 265: Microsoft.Kiota.Abstractions => 0x54c3be10 => 219
	i32 1422545099, ; 266: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 267: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 404
	i32 1433579683, ; 268: de\Microsoft.CodeAnalysis.resources => 0x5572b0a3 => 353
	i32 1434145427, ; 269: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 270: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 341
	i32 1439761251, ; 271: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1446836800, ; 272: it\Microsoft.CodeAnalysis.resources => 0x563cfa40 => 356
	i32 1451017060, ; 273: cs/Microsoft.CodeAnalysis.resources.dll => 0x567cc364 => 352
	i32 1452070440, ; 274: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 275: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 276: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 277: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1460893475, ; 278: System.IdentityModel.Tokens.Jwt => 0x57137723 => 262
	i32 1461004990, ; 279: es\Microsoft.Maui.Controls.resources => 0x57152abe => 410
	i32 1461234159, ; 280: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 281: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 282: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 283: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 278
	i32 1470490898, ; 284: Microsoft.Extensions.Primitives => 0x57a5e912 => 209
	i32 1474151183, ; 285: de/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x57ddc30f => 379
	i32 1479771757, ; 286: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 287: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 288: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1487250139, ; 289: K4os.Hash.xxHash => 0x58a5a2db => 181
	i32 1490025113, ; 290: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 325
	i32 1490351284, ; 291: Microsoft.Data.Sqlite.dll => 0x58d4f4b4 => 192
	i32 1493001747, ; 292: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 414
	i32 1498168481, ; 293: Microsoft.IdentityModel.JsonWebTokens.dll => 0x594c3ca1 => 213
	i32 1511525525, ; 294: MySqlConnector => 0x5a180c95 => 233
	i32 1513823142, ; 295: Serilog.Settings.Configuration => 0x5a3b1ba6 => 239
	i32 1514721132, ; 296: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 409
	i32 1515641136, ; 297: es\Microsoft.CodeAnalysis.CSharp.resources => 0x5a56d930 => 367
	i32 1525768991, ; 298: ru\Microsoft.CodeAnalysis.resources => 0x5af1631f => 361
	i32 1534512859, ; 299: Std.UriTemplate => 0x5b76cedb => 249
	i32 1536373174, ; 300: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 301: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 302: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 303: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 304: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 429
	i32 1564802854, ; 305: SQLitePCLRaw.provider.e_sqlcipher.dll => 0x5d44ff26 => 247
	i32 1565862583, ; 306: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566167090, ; 307: zh-Hant/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x5d59d032 => 377
	i32 1566207040, ; 308: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1568109833, ; 309: fr/Microsoft.CodeAnalysis.resources.dll => 0x5d777509 => 355
	i32 1573516615, ; 310: zh-Hans\Microsoft.CodeAnalysis.Workspaces.resources => 0x5dc9f547 => 402
	i32 1573704789, ; 311: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 312: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 313: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 294
	i32 1592978981, ; 314: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 315: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 342
	i32 1601112923, ; 316: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 317: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 318: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 319: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 314
	i32 1622358360, ; 320: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 321: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 336
	i32 1625558452, ; 322: Serilog.dll => 0x60e40db4 => 237
	i32 1628113371, ; 323: Microsoft.IdentityModel.Protocols.OpenIdConnect => 0x610b09db => 216
	i32 1635184631, ; 324: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 298
	i32 1636350590, ; 325: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 291
	i32 1639515021, ; 326: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 327: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 328: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 329: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 330: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 330
	i32 1658251792, ; 331: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 339
	i32 1665647309, ; 332: pt-BR/Microsoft.CodeAnalysis.resources.dll => 0x6347c2cd => 360
	i32 1670060433, ; 333: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 286
	i32 1675553242, ; 334: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 335: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 336: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 337: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1685661845, ; 338: pl\Microsoft.CodeAnalysis.Workspaces.resources => 0x64792895 => 398
	i32 1688112883, ; 339: Microsoft.Data.Sqlite => 0x649e8ef3 => 192
	i32 1689493916, ; 340: Microsoft.EntityFrameworkCore.dll => 0x64b3a19c => 193
	i32 1691477237, ; 341: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 342: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 343: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 346
	i32 1701541528, ; 344: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1703937184, ; 345: Microsoft.CodeAnalysis.VisualBasic.Features.dll => 0x659004a0 => 189
	i32 1711441057, ; 346: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 246
	i32 1712031326, ; 347: System.Composition.Hosting.dll => 0x660b865e => 257
	i32 1720223769, ; 348: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 307
	i32 1726116996, ; 349: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 350: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 351: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 282
	i32 1736233607, ; 352: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 427
	i32 1743415430, ; 353: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 405
	i32 1744735666, ; 354: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 355: System.IO.Pipelines.dll => 0x68139a0d => 263
	i32 1746316138, ; 356: Mono.Android.Export => 0x6816ab6a => 169
	i32 1746428678, ; 357: it/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x68186306 => 369
	i32 1746610600, ; 358: tr/Microsoft.CodeAnalysis.resources.dll => 0x681b29a8 => 362
	i32 1750313021, ; 359: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1757062295, ; 360: tr/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x68baa497 => 401
	i32 1758240030, ; 361: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 362: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 363: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 364: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 329
	i32 1767875481, ; 365: ko/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x695fa399 => 397
	i32 1770582343, ; 366: Microsoft.Extensions.Logging.dll => 0x6988f147 => 205
	i32 1776026572, ; 367: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 368: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 369: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1781174529, ; 370: zh-Hant\Microsoft.CodeAnalysis.CSharp.resources => 0x6a2a9101 => 377
	i32 1782862114, ; 371: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 421
	i32 1788241197, ; 372: Xamarin.AndroidX.Fragment => 0x6a96652d => 300
	i32 1793755602, ; 373: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 413
	i32 1796167890, ; 374: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 182
	i32 1808609942, ; 375: Xamarin.AndroidX.Loader => 0x6bcd3296 => 314
	i32 1813058853, ; 376: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 345
	i32 1813201214, ; 377: Xamarin.Google.Android.Material => 0x6c13413e => 339
	i32 1818569960, ; 378: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 319
	i32 1818787751, ; 379: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 380: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 381: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1828688058, ; 382: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 206
	i32 1829150748, ; 383: System.Windows.Extensions => 0x6d06a01c => 267
	i32 1842015223, ; 384: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 433
	i32 1847515442, ; 385: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 269
	i32 1853025655, ; 386: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 430
	i32 1858542181, ; 387: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 388: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1871986876, ; 389: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 0x6f9440bc => 216
	i32 1875935024, ; 390: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 412
	i32 1879696579, ; 391: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 392: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 280
	i32 1886040351, ; 393: Microsoft.EntityFrameworkCore.Sqlite.dll => 0x706ab11f => 196
	i32 1888955245, ; 394: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 395: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 396: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 397: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 398: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1925302748, ; 399: K4os.Compression.LZ4.dll => 0x72c1c9dc => 179
	i32 1928716804, ; 400: ja\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x72f5e204 => 383
	i32 1931959220, ; 401: System.Composition.AttributedModel.dll => 0x73275bb4 => 255
	i32 1937339544, ; 402: it/Microsoft.CodeAnalysis.resources.dll => 0x73797498 => 356
	i32 1939592360, ; 403: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1942190367, ; 404: it\Microsoft.CodeAnalysis.CSharp.resources => 0x73c3791f => 369
	i32 1956758971, ; 405: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1958201531, ; 406: tr\Microsoft.CodeAnalysis.CSharp.resources => 0x74b7c8bb => 375
	i32 1961813231, ; 407: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 326
	i32 1968388702, ; 408: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 199
	i32 1968930087, ; 409: pt-BR\Microsoft.CodeAnalysis.CSharp.resources => 0x755b7d27 => 373
	i32 1983156543, ; 410: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 346
	i32 1983665899, ; 411: System.Composition.TypedParts.dll => 0x763c56eb => 259
	i32 1985761444, ; 412: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 271
	i32 1986222447, ; 413: Microsoft.IdentityModel.Tokens.dll => 0x7663596f => 217
	i32 1993867835, ; 414: ko\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x76d8023b => 384
	i32 1993916150, ; 415: ko\Microsoft.CodeAnalysis.resources => 0x76d8bef6 => 358
	i32 2002441971, ; 416: it/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x775ad6f3 => 395
	i32 2003115576, ; 417: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 409
	i32 2011961780, ; 418: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2014489277, ; 419: Microsoft.EntityFrameworkCore.Sqlite => 0x7812aabd => 196
	i32 2019465201, ; 420: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 311
	i32 2025202353, ; 421: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 404
	i32 2031763787, ; 422: Xamarin.Android.Glide => 0x791a414b => 268
	i32 2045470958, ; 423: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 424: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 201
	i32 2048711491, ; 425: pl/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x7a1cdb43 => 398
	i32 2055257422, ; 426: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 306
	i32 2060060697, ; 427: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 428: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 408
	i32 2070888862, ; 429: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 430: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2084242321, ; 431: es/Microsoft.CodeAnalysis.resources.dll => 0x7c3b0391 => 354
	i32 2090596640, ; 432: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 433: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 248
	i32 2113869790, ; 434: Microsoft.Graph => 0x7dff17de => 210
	i32 2127167465, ; 435: System.Console => 0x7ec9ffe9 => 20
	i32 2138035031, ; 436: ko/Microsoft.CodeAnalysis.resources.dll => 0x7f6fd357 => 358
	i32 2141478350, ; 437: ko\Microsoft.CodeAnalysis.Workspaces.resources => 0x7fa45dce => 397
	i32 2142473426, ; 438: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 439: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 440: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 441: Microsoft.Maui => 0x80bd55ad => 229
	i32 2169148018, ; 442: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 416
	i32 2171397733, ; 443: Serilog.Sinks.Console.dll => 0x816ce665 => 240
	i32 2181485124, ; 444: Serilog.Sinks.File => 0x8206d244 => 241
	i32 2181898931, ; 445: Microsoft.Extensions.Options.dll => 0x820d22b3 => 208
	i32 2192057212, ; 446: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 206
	i32 2193016926, ; 447: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2197979891, ; 448: Microsoft.Extensions.DependencyModel.dll => 0x830282f3 => 204
	i32 2201107256, ; 449: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 350
	i32 2201231467, ; 450: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 451: it\Microsoft.Maui.Controls.resources => 0x839595db => 418
	i32 2217644978, ; 452: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 333
	i32 2222056684, ; 453: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 454: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 315
	i32 2252106437, ; 455: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2252897993, ; 456: Microsoft.EntityFrameworkCore => 0x86487ec9 => 193
	i32 2253551641, ; 457: Microsoft.IdentityModel.Protocols => 0x86527819 => 215
	i32 2256313426, ; 458: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 459: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 460: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 200
	i32 2267999099, ; 461: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 270
	i32 2270573516, ; 462: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 412
	i32 2279755925, ; 463: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 322
	i32 2293034957, ; 464: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2293126107, ; 465: pl\Microsoft.CodeAnalysis.resources => 0x88ae53db => 359
	i32 2295906218, ; 466: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 467: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 468: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 422
	i32 2305521784, ; 469: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 470: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 274
	i32 2320631194, ; 471: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2323173394, ; 472: ja\Microsoft.CodeAnalysis.Workspaces.resources => 0x8a78d012 => 396
	i32 2334514644, ; 473: pt-BR\Microsoft.CodeAnalysis.Workspaces.resources => 0x8b25ddd4 => 399
	i32 2340441535, ; 474: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 475: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 476: System.Net.Primitives => 0x8c40e0db => 70
	i32 2354730003, ; 477: Syncfusion.Licensing => 0x8c5a5413 => 250
	i32 2358249420, ; 478: Serilog.Extensions.Logging => 0x8c9007cc => 238
	i32 2368005991, ; 479: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2369706906, ; 480: Microsoft.IdentityModel.Logging => 0x8d3edb9a => 214
	i32 2371007202, ; 481: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 199
	i32 2378619854, ; 482: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 483: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 484: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 417
	i32 2401565422, ; 485: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 486: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 297
	i32 2421380589, ; 487: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 488: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 284
	i32 2427813419, ; 489: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 414
	i32 2435356389, ; 490: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 491: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2454642406, ; 492: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458475166, ; 493: cs\Microsoft.CodeAnalysis.resources => 0x92895a9e => 352
	i32 2458678730, ; 494: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 495: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2462337112, ; 496: tr\Microsoft.CodeAnalysis.Workspaces.resources => 0x92c44858 => 401
	i32 2465273461, ; 497: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 243
	i32 2465532216, ; 498: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 287
	i32 2471149680, ; 499: Bingie => 0x934ac070 => 0
	i32 2471841756, ; 500: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 501: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 502: Microsoft.Maui.Controls => 0x93dba8a1 => 227
	i32 2483903535, ; 503: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 504: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2486824558, ; 505: K4os.Hash.xxHash.dll => 0x9439ee6e => 181
	i32 2490993605, ; 506: System.AppContext.dll => 0x94798bc5 => 6
	i32 2498657740, ; 507: BouncyCastle.Cryptography.dll => 0x94ee7dcc => 175
	i32 2501346920, ; 508: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 509: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 309
	i32 2509217888, ; 510: System.Diagnostics.EventLog => 0x958fa060 => 261
	i32 2511706353, ; 511: Microsoft.CodeAnalysis.Features => 0x95b598f1 => 187
	i32 2518448533, ; 512: fr\Microsoft.CodeAnalysis.Workspaces.resources => 0x961c7995 => 394
	i32 2519222276, ; 513: Syncfusion.Maui.Calendar => 0x96284804 => 251
	i32 2522472828, ; 514: Xamarin.Android.Glide.dll => 0x9659e17c => 268
	i32 2538310050, ; 515: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550034082, ; 516: Microsoft.Kiota.Abstractions.dll => 0x97fe6ea2 => 219
	i32 2550873716, ; 517: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 415
	i32 2562349572, ; 518: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 519: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 520: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 310
	i32 2581819634, ; 521: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 332
	i32 2583428798, ; 522: pt-BR/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x99fbfebe => 399
	i32 2585220780, ; 523: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 524: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 525: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2592999858, ; 526: pl/Microsoft.CodeAnalysis.resources.dll => 0x9a8e09b2 => 359
	i32 2593496499, ; 527: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 424
	i32 2597137376, ; 528: es/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x9acd2be0 => 367
	i32 2605712449, ; 529: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 350
	i32 2611359322, ; 530: ZstdSharp.dll => 0x9ba62e5a => 351
	i32 2615233544, ; 531: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 301
	i32 2616218305, ; 532: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 207
	i32 2617129537, ; 533: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 534: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 535: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 291
	i32 2624644809, ; 536: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 296
	i32 2626831493, ; 537: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 419
	i32 2627185994, ; 538: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2627802292, ; 539: Serilog.Extensions.Logging.dll => 0x9ca114b4 => 238
	i32 2628210652, ; 540: System.Memory.Data => 0x9ca74fdc => 264
	i32 2629843544, ; 541: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 542: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 305
	i32 2634653062, ; 543: Microsoft.EntityFrameworkCore.Relational.dll => 0x9d099d86 => 195
	i32 2640290731, ; 544: Microsoft.IdentityModel.Logging.dll => 0x9d5fa3ab => 214
	i32 2640706905, ; 545: Azure.Core => 0x9d65fd59 => 173
	i32 2648230441, ; 546: tr/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x9dd8ca29 => 375
	i32 2660759594, ; 547: System.Security.Cryptography.ProtectedData.dll => 0x9e97f82a => 265
	i32 2663391936, ; 548: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 270
	i32 2663698177, ; 549: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 550: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 551: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 552: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 553: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 554: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 555: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 330
	i32 2715334215, ; 556: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 557: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 558: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2722434549, ; 559: Microsoft.CodeAnalysis.dll => 0xa2450df5 => 183
	i32 2723680174, ; 560: es/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xa2580fae => 393
	i32 2724373263, ; 561: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 562: Xamarin.AndroidX.Activity => 0xa2e0939b => 272
	i32 2735172069, ; 563: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 564: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 278
	i32 2740948882, ; 565: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 566: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 567: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 425
	i32 2758225723, ; 568: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 228
	i32 2764765095, ; 569: Microsoft.Maui.dll => 0xa4caf7a7 => 229
	i32 2765824710, ; 570: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 571: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 344
	i32 2778768386, ; 572: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 335
	i32 2779977773, ; 573: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 323
	i32 2785988530, ; 574: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 431
	i32 2788224221, ; 575: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 301
	i32 2801831435, ; 576: Microsoft.Maui.Graphics => 0xa7008e0b => 231
	i32 2803228030, ; 577: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806077508, ; 578: Microsoft.Graph.dll => 0xa7415844 => 210
	i32 2806116107, ; 579: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 410
	i32 2810250172, ; 580: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 288
	i32 2819470561, ; 581: System.Xml.dll => 0xa80db4e1 => 163
	i32 2819745351, ; 582: pt-BR/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xa811e647 => 373
	i32 2821205001, ; 583: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 584: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 323
	i32 2823470325, ; 585: pt-BR/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xa84abcf5 => 386
	i32 2824502124, ; 586: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2831556043, ; 587: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 423
	i32 2838993487, ; 588: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 312
	i32 2841355853, ; 589: System.Security.Permissions => 0xa95ba64d => 266
	i32 2847789619, ; 590: Microsoft.EntityFrameworkCore.Relational => 0xa9bdd233 => 195
	i32 2849599387, ; 591: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 592: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 335
	i32 2855708567, ; 593: Xamarin.AndroidX.Transition => 0xaa36a797 => 331
	i32 2861098320, ; 594: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 595: Microsoft.Maui.Essentials => 0xaa8a4878 => 230
	i32 2867142744, ; 596: ja/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xaae52058 => 370
	i32 2867946736, ; 597: System.Security.Cryptography.ProtectedData => 0xaaf164f0 => 265
	i32 2868557005, ; 598: Syncfusion.Licensing.dll => 0xaafab4cd => 250
	i32 2870099610, ; 599: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 273
	i32 2875164099, ; 600: Jsr305Binding.dll => 0xab5f85c3 => 340
	i32 2875220617, ; 601: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2875299966, ; 602: Microsoft.Kiota.Http.HttpClientLibrary.dll => 0xab61987e => 221
	i32 2884993177, ; 603: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 299
	i32 2887636118, ; 604: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 605: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 606: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 607: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 608: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 609: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2916838712, ; 610: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 336
	i32 2919462931, ; 611: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 612: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 275
	i32 2936416060, ; 613: System.Resources.Reader => 0xaf06273c => 98
	i32 2938903864, ; 614: Microsoft.Kiota.Serialization.Multipart => 0xaf2c1d38 => 224
	i32 2940926066, ; 615: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 616: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2944313911, ; 617: System.Configuration.ConfigurationManager.dll => 0xaf7eaa37 => 260
	i32 2959614098, ; 618: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 619: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2970759306, ; 620: BCrypt.Net-Next.dll => 0xb112308a => 174
	i32 2972252294, ; 621: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 622: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 295
	i32 2987532451, ; 623: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 326
	i32 2988176241, ; 624: Microsoft.CodeAnalysis.Workspaces => 0xb21bf371 => 191
	i32 2996846495, ; 625: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 308
	i32 3012788804, ; 626: System.Configuration.ConfigurationManager => 0xb3938244 => 260
	i32 3016983068, ; 627: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 328
	i32 3023353419, ; 628: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 629: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 303
	i32 3025069135, ; 630: K4os.Compression.LZ4.Streams.dll => 0xb44ee44f => 180
	i32 3033605958, ; 631: System.Memory.Data.dll => 0xb4d12746 => 264
	i32 3038032645, ; 632: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 438
	i32 3056245963, ; 633: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 325
	i32 3057625584, ; 634: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 316
	i32 3058348817, ; 635: es\Microsoft.CodeAnalysis.Workspaces.resources => 0xb64ab311 => 393
	i32 3059408633, ; 636: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 637: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3069363400, ; 638: Microsoft.Extensions.Caching.Abstractions.dll => 0xb6f2c4c8 => 197
	i32 3072861693, ; 639: Microsoft.Kiota.Serialization.Form.dll => 0xb72825fd => 222
	i32 3075834255, ; 640: System.Threading.Tasks => 0xb755818f => 144
	i32 3077302341, ; 641: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 416
	i32 3077674723, ; 642: ru/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xb77196e3 => 400
	i32 3084678329, ; 643: Microsoft.IdentityModel.Tokens => 0xb7dc74b9 => 217
	i32 3089219899, ; 644: ZstdSharp => 0xb821c13b => 351
	i32 3090735792, ; 645: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 646: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 647: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 648: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 649: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 650: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 651: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3137307937, ; 652: fr\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xbaff8521 => 381
	i32 3147165239, ; 653: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3147228406, ; 654: Syncfusion.Maui.Core => 0xbb96e4f6 => 252
	i32 3148237826, ; 655: GoogleGson.dll => 0xbba64c02 => 177
	i32 3159123045, ; 656: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 657: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3162329233, ; 658: zh-Hans\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xbc7d5091 => 389
	i32 3173713272, ; 659: cs/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xbd2b0578 => 378
	i32 3176689669, ; 660: Microsoft.IdentityModel.Validators.dll => 0xbd587005 => 218
	i32 3178803400, ; 661: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 317
	i32 3192346100, ; 662: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 663: System.Web => 0xbe592c0c => 153
	i32 3195844289, ; 664: Microsoft.Extensions.Caching.Abstractions => 0xbe7cb6c1 => 197
	i32 3204380047, ; 665: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 666: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 667: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 294
	i32 3213246214, ; 668: System.Security.Permissions.dll => 0xbf863f06 => 266
	i32 3220365878, ; 669: System.Threading => 0xbff2e236 => 148
	i32 3222040828, ; 670: Microsoft.IdentityModel.Validators => 0xc00c70fc => 218
	i32 3226221578, ; 671: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3245785879, ; 672: de/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xc176c317 => 366
	i32 3251039220, ; 673: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 674: Xamarin.AndroidX.CardView => 0xc235e84d => 282
	i32 3265493905, ; 675: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 676: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 677: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 678: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 679: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3284892224, ; 680: ru/Microsoft.CodeAnalysis.resources.dll => 0xc3cb7a40 => 361
	i32 3286872994, ; 681: SQLite-net.dll => 0xc3e9b3a2 => 242
	i32 3290767353, ; 682: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 683: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 684: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3304979567, ; 685: Humanizer.dll => 0xc4fdfc6f => 178
	i32 3305363605, ; 686: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 411
	i32 3312457198, ; 687: Microsoft.IdentityModel.JsonWebTokens => 0xc57015ee => 213
	i32 3315699264, ; 688: System.Composition.Hosting => 0xc5a18e40 => 257
	i32 3316684772, ; 689: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 690: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 292
	i32 3317144872, ; 691: System.Data => 0xc5b79d28 => 24
	i32 3329719219, ; 692: Microsoft.CodeAnalysis.VisualBasic.Features => 0xc6777bb3 => 189
	i32 3340431453, ; 693: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 280
	i32 3345895724, ; 694: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 321
	i32 3346324047, ; 695: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 318
	i32 3357674450, ; 696: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 428
	i32 3358260929, ; 697: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 698: SQLitePCLRaw.core => 0xc849ca45 => 244
	i32 3362336904, ; 699: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 273
	i32 3362522851, ; 700: Xamarin.AndroidX.Core => 0xc86c06e3 => 289
	i32 3366347497, ; 701: Java.Interop => 0xc8a662e9 => 168
	i32 3366973124, ; 702: zh-Hans\Microsoft.CodeAnalysis.resources => 0xc8afeec4 => 363
	i32 3367815115, ; 703: Microsoft.Graph.Core.dll => 0xc8bcc7cb => 211
	i32 3374879918, ; 704: Microsoft.IdentityModel.Protocols.dll => 0xc92894ae => 215
	i32 3374999561, ; 705: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 322
	i32 3381016424, ; 706: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 407
	i32 3381033598, ; 707: K4os.Compression.LZ4 => 0xc9867a7e => 179
	i32 3385193906, ; 708: it\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xc9c5f5b2 => 382
	i32 3395150330, ; 709: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3396351408, ; 710: ru\Microsoft.CodeAnalysis.CSharp.resources => 0xca7035b0 => 374
	i32 3403906625, ; 711: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 712: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 293
	i32 3421170118, ; 713: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 201
	i32 3428513518, ; 714: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 202
	i32 3429136800, ; 715: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 716: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 717: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 296
	i32 3445260447, ; 718: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 719: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 226
	i32 3454422360, ; 720: Microsoft.Kiota.Serialization.Multipart.dll => 0xcde64d58 => 224
	i32 3457410834, ; 721: es/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xce13e712 => 380
	i32 3463511458, ; 722: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 415
	i32 3467345667, ; 723: MySql.Data => 0xceab7f03 => 232
	i32 3471940407, ; 724: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3472012038, ; 725: BCrypt.Net-Next => 0xcef2b306 => 174
	i32 3476120550, ; 726: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 727: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 428
	i32 3484440000, ; 728: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 427
	i32 3485117614, ; 729: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 730: System.Transactions => 0xcfd0c798 => 150
	i32 3488260661, ; 731: pt-BR\Microsoft.CodeAnalysis.resources => 0xcfeaa235 => 360
	i32 3493954962, ; 732: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 285
	i32 3499097210, ; 733: Google.Protobuf.dll => 0xd08ffc7a => 176
	i32 3509114376, ; 734: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 735: System.Security.dll => 0xd1854eb4 => 130
	i32 3519914379, ; 736: Microsoft.CodeAnalysis.CSharp.Workspaces.dll => 0xd1cda18b => 186
	i32 3520934708, ; 737: Microsoft.Kiota.Serialization.Form => 0xd1dd3334 => 222
	i32 3530783736, ; 738: zh-Hans/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xd2737bf8 => 389
	i32 3530912306, ; 739: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 740: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3545363771, ; 741: System.Composition.AttributedModel => 0xd351f53b => 255
	i32 3558648585, ; 742: System.ClientModel => 0xd41cab09 => 254
	i32 3560100363, ; 743: System.Threading.Timer => 0xd432d20b => 147
	i32 3561949811, ; 744: Azure.Core.dll => 0xd44f0a73 => 173
	i32 3570554715, ; 745: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 746: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 435
	i32 3597029428, ; 747: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 271
	i32 3598340787, ; 748: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3605570793, ; 749: BouncyCastle.Cryptography => 0xd6e8a4e9 => 175
	i32 3608519521, ; 750: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 751: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 752: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 320
	i32 3633644679, ; 753: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 275
	i32 3638274909, ; 754: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 755: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 306
	i32 3643446276, ; 756: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 432
	i32 3643854240, ; 757: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 317
	i32 3645089577, ; 758: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3645630983, ; 759: Google.Protobuf => 0xd94bea07 => 176
	i32 3657292374, ; 760: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 200
	i32 3657523560, ; 761: Bingie.dll => 0xda016168 => 0
	i32 3660523487, ; 762: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 763: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 764: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 281
	i32 3684561358, ; 765: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 285
	i32 3685965367, ; 766: ja/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xdbb35e37 => 383
	i32 3687915678, ; 767: Microsoft.Kiota.Http.HttpClientLibrary => 0xdbd1209e => 221
	i32 3697841164, ; 768: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 437
	i32 3700591436, ; 769: Microsoft.IdentityModel.Abstractions.dll => 0xdc928b4c => 212
	i32 3700866549, ; 770: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3702852043, ; 771: ko\Microsoft.CodeAnalysis.CSharp.resources => 0xdcb509cb => 371
	i32 3706696989, ; 772: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 290
	i32 3716563718, ; 773: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 774: Xamarin.AndroidX.Annotation => 0xdda814c6 => 274
	i32 3724971120, ; 775: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 316
	i32 3726828805, ; 776: de\Microsoft.CodeAnalysis.Workspaces.resources => 0xde22e505 => 392
	i32 3732100267, ; 777: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 778: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748220531, ; 779: fr\Microsoft.CodeAnalysis.resources => 0xdf694e73 => 355
	i32 3748608112, ; 780: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 781: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3754087143, ; 782: Microsoft.CodeAnalysis.CSharp.Features => 0xdfc2d2e7 => 185
	i32 3754567612, ; 783: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 248
	i32 3786282454, ; 784: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 283
	i32 3792276235, ; 785: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 786: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 226
	i32 3802395368, ; 787: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3808703996, ; 788: ru\Microsoft.CodeAnalysis.Workspaces.resources => 0xe30435fc => 400
	i32 3819260425, ; 789: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 790: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3826434917, ; 791: Microsoft.Graph.Core => 0xe412c365 => 211
	i32 3829621856, ; 792: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 793: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 203
	i32 3844307129, ; 794: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 795: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 796: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3871054620, ; 797: System.Composition.Runtime => 0xe6bb9b1c => 258
	i32 3873536506, ; 798: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 799: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 800: SQLite-net => 0xe70c9739 => 242
	i32 3885497537, ; 801: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 802: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 331
	i32 3888767677, ; 803: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 321
	i32 3889960447, ; 804: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 436
	i32 3896106733, ; 805: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 806: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 289
	i32 3900683505, ; 807: System.Composition.TypedParts => 0xe87fb4f1 => 259
	i32 3901907137, ; 808: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3901960326, ; 809: ko/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xe8933086 => 384
	i32 3920810846, ; 810: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 811: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 334
	i32 3928044579, ; 812: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 813: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 814: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 319
	i32 3941902297, ; 815: Roslynator.Interfaces => 0xeaf4a7d9 => 236
	i32 3945713374, ; 816: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 817: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 818: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 277
	i32 3959773229, ; 819: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 308
	i32 3980434154, ; 820: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 431
	i32 3987592930, ; 821: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 413
	i32 4000339556, ; 822: ko/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xee705664 => 371
	i32 4002654803, ; 823: Microsoft.CodeAnalysis.Workspaces.dll => 0xee93aa53 => 191
	i32 4003436829, ; 824: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4006106083, ; 825: pl\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xeec853e3 => 385
	i32 4014412877, ; 826: Microsoft.Kiota.Serialization.Json.dll => 0xef47144d => 223
	i32 4015948917, ; 827: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 276
	i32 4023392905, ; 828: System.IO.Pipelines => 0xefd01a89 => 263
	i32 4023689262, ; 829: fr/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xefd4a02e => 381
	i32 4025784931, ; 830: System.Memory => 0xeff49a63 => 62
	i32 4026527876, ; 831: Microsoft.CodeAnalysis.CSharp => 0xeffff084 => 184
	i32 4035041792, ; 832: zh-Hant/Microsoft.CodeAnalysis.resources.dll => 0xf081da00 => 364
	i32 4035816735, ; 833: es\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xf08dad1f => 380
	i32 4046471985, ; 834: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 228
	i32 4054681211, ; 835: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 836: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 837: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4076551833, ; 838: Std.UriTemplate.dll => 0xf2fb3e99 => 249
	i32 4079385022, ; 839: MySqlConnector.dll => 0xf32679be => 233
	i32 4094352644, ; 840: Microsoft.Maui.Essentials.dll => 0xf40add04 => 230
	i32 4099507663, ; 841: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 842: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 843: Xamarin.AndroidX.Emoji2 => 0xf479582c => 297
	i32 4101842092, ; 844: Microsoft.Extensions.Caching.Memory => 0xf47d24ac => 198
	i32 4102112229, ; 845: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 426
	i32 4118796114, ; 846: de\Microsoft.CodeAnalysis.CSharp.resources => 0xf57fd752 => 366
	i32 4125707920, ; 847: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 421
	i32 4126470640, ; 848: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 202
	i32 4127667938, ; 849: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 850: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 851: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 852: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 433
	i32 4151237749, ; 853: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 854: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4160752180, ; 855: Microsoft.CodeAnalysis.VisualBasic => 0xf8000a34 => 188
	i32 4161255271, ; 856: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164470604, ; 857: tr\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xf838c74c => 388
	i32 4164802419, ; 858: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4177402947, ; 859: cs/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xf8fe1c43 => 365
	i32 4181436372, ; 860: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 861: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 313
	i32 4185676441, ; 862: System.Security => 0xf97c5a99 => 130
	i32 4187874287, ; 863: tr\Microsoft.CodeAnalysis.resources => 0xf99de3ef => 362
	i32 4196171640, ; 864: Microsoft.CodeAnalysis => 0xfa1c7f78 => 183
	i32 4196529839, ; 865: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4207745933, ; 866: it\Microsoft.CodeAnalysis.Workspaces.resources => 0xfacd1b8d => 395
	i32 4213026141, ; 867: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4245218886, ; 868: Microsoft.CodeAnalysis.CSharp.dll => 0xfd08e646 => 184
	i32 4254412227, ; 869: SQLitePCLRaw.provider.e_sqlcipher => 0xfd952dc3 => 247
	i32 4256097574, ; 870: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 290
	i32 4258378803, ; 871: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 312
	i32 4260525087, ; 872: System.Buffers => 0xfdf2741f => 7
	i32 4263231520, ; 873: System.IdentityModel.Tokens.Jwt.dll => 0xfe1bc020 => 262
	i32 4271975918, ; 874: Microsoft.Maui.Controls.dll => 0xfea12dee => 227
	i32 4274976490, ; 875: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 876: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 313
	i32 4294763496 ; 877: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 299
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [878 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 188, ; 3
	i32 204, ; 4
	i32 235, ; 5
	i32 353, ; 6
	i32 309, ; 7
	i32 343, ; 8
	i32 48, ; 9
	i32 403, ; 10
	i32 80, ; 11
	i32 145, ; 12
	i32 379, ; 13
	i32 236, ; 14
	i32 30, ; 15
	i32 437, ; 16
	i32 124, ; 17
	i32 231, ; 18
	i32 186, ; 19
	i32 102, ; 20
	i32 363, ; 21
	i32 327, ; 22
	i32 107, ; 23
	i32 327, ; 24
	i32 139, ; 25
	i32 347, ; 26
	i32 234, ; 27
	i32 77, ; 28
	i32 124, ; 29
	i32 13, ; 30
	i32 283, ; 31
	i32 252, ; 32
	i32 132, ; 33
	i32 329, ; 34
	i32 151, ; 35
	i32 225, ; 36
	i32 434, ; 37
	i32 435, ; 38
	i32 18, ; 39
	i32 281, ; 40
	i32 26, ; 41
	i32 235, ; 42
	i32 303, ; 43
	i32 1, ; 44
	i32 59, ; 45
	i32 42, ; 46
	i32 376, ; 47
	i32 91, ; 48
	i32 378, ; 49
	i32 185, ; 50
	i32 286, ; 51
	i32 178, ; 52
	i32 147, ; 53
	i32 305, ; 54
	i32 302, ; 55
	i32 406, ; 56
	i32 54, ; 57
	i32 368, ; 58
	i32 69, ; 59
	i32 434, ; 60
	i32 272, ; 61
	i32 83, ; 62
	i32 386, ; 63
	i32 419, ; 64
	i32 304, ; 65
	i32 246, ; 66
	i32 418, ; 67
	i32 131, ; 68
	i32 55, ; 69
	i32 253, ; 70
	i32 149, ; 71
	i32 74, ; 72
	i32 145, ; 73
	i32 391, ; 74
	i32 62, ; 75
	i32 256, ; 76
	i32 146, ; 77
	i32 438, ; 78
	i32 240, ; 79
	i32 165, ; 80
	i32 376, ; 81
	i32 364, ; 82
	i32 388, ; 83
	i32 430, ; 84
	i32 287, ; 85
	i32 12, ; 86
	i32 300, ; 87
	i32 125, ; 88
	i32 152, ; 89
	i32 113, ; 90
	i32 166, ; 91
	i32 164, ; 92
	i32 302, ; 93
	i32 212, ; 94
	i32 315, ; 95
	i32 84, ; 96
	i32 417, ; 97
	i32 411, ; 98
	i32 225, ; 99
	i32 209, ; 100
	i32 390, ; 101
	i32 150, ; 102
	i32 347, ; 103
	i32 60, ; 104
	i32 190, ; 105
	i32 205, ; 106
	i32 51, ; 107
	i32 382, ; 108
	i32 103, ; 109
	i32 114, ; 110
	i32 182, ; 111
	i32 40, ; 112
	i32 340, ; 113
	i32 338, ; 114
	i32 120, ; 115
	i32 425, ; 116
	i32 220, ; 117
	i32 52, ; 118
	i32 44, ; 119
	i32 119, ; 120
	i32 180, ; 121
	i32 292, ; 122
	i32 423, ; 123
	i32 298, ; 124
	i32 81, ; 125
	i32 136, ; 126
	i32 334, ; 127
	i32 279, ; 128
	i32 8, ; 129
	i32 223, ; 130
	i32 73, ; 131
	i32 256, ; 132
	i32 405, ; 133
	i32 155, ; 134
	i32 349, ; 135
	i32 154, ; 136
	i32 253, ; 137
	i32 92, ; 138
	i32 344, ; 139
	i32 45, ; 140
	i32 385, ; 141
	i32 420, ; 142
	i32 408, ; 143
	i32 234, ; 144
	i32 348, ; 145
	i32 109, ; 146
	i32 254, ; 147
	i32 396, ; 148
	i32 129, ; 149
	i32 239, ; 150
	i32 243, ; 151
	i32 25, ; 152
	i32 269, ; 153
	i32 72, ; 154
	i32 55, ; 155
	i32 46, ; 156
	i32 429, ; 157
	i32 258, ; 158
	i32 208, ; 159
	i32 293, ; 160
	i32 354, ; 161
	i32 22, ; 162
	i32 307, ; 163
	i32 237, ; 164
	i32 86, ; 165
	i32 43, ; 166
	i32 160, ; 167
	i32 71, ; 168
	i32 320, ; 169
	i32 3, ; 170
	i32 42, ; 171
	i32 63, ; 172
	i32 16, ; 173
	i32 190, ; 174
	i32 53, ; 175
	i32 251, ; 176
	i32 432, ; 177
	i32 343, ; 178
	i32 387, ; 179
	i32 105, ; 180
	i32 368, ; 181
	i32 348, ; 182
	i32 341, ; 183
	i32 304, ; 184
	i32 34, ; 185
	i32 158, ; 186
	i32 232, ; 187
	i32 85, ; 188
	i32 32, ; 189
	i32 12, ; 190
	i32 51, ; 191
	i32 56, ; 192
	i32 324, ; 193
	i32 36, ; 194
	i32 203, ; 195
	i32 407, ; 196
	i32 342, ; 197
	i32 277, ; 198
	i32 35, ; 199
	i32 58, ; 200
	i32 311, ; 201
	i32 245, ; 202
	i32 387, ; 203
	i32 177, ; 204
	i32 245, ; 205
	i32 17, ; 206
	i32 345, ; 207
	i32 372, ; 208
	i32 261, ; 209
	i32 164, ; 210
	i32 370, ; 211
	i32 420, ; 212
	i32 310, ; 213
	i32 207, ; 214
	i32 267, ; 215
	i32 337, ; 216
	i32 194, ; 217
	i32 403, ; 218
	i32 426, ; 219
	i32 153, ; 220
	i32 333, ; 221
	i32 372, ; 222
	i32 318, ; 223
	i32 187, ; 224
	i32 194, ; 225
	i32 424, ; 226
	i32 279, ; 227
	i32 198, ; 228
	i32 29, ; 229
	i32 52, ; 230
	i32 392, ; 231
	i32 422, ; 232
	i32 338, ; 233
	i32 5, ; 234
	i32 406, ; 235
	i32 328, ; 236
	i32 332, ; 237
	i32 284, ; 238
	i32 349, ; 239
	i32 276, ; 240
	i32 244, ; 241
	i32 295, ; 242
	i32 357, ; 243
	i32 85, ; 244
	i32 394, ; 245
	i32 337, ; 246
	i32 241, ; 247
	i32 61, ; 248
	i32 390, ; 249
	i32 402, ; 250
	i32 391, ; 251
	i32 112, ; 252
	i32 357, ; 253
	i32 57, ; 254
	i32 374, ; 255
	i32 436, ; 256
	i32 365, ; 257
	i32 324, ; 258
	i32 99, ; 259
	i32 19, ; 260
	i32 288, ; 261
	i32 111, ; 262
	i32 101, ; 263
	i32 220, ; 264
	i32 219, ; 265
	i32 102, ; 266
	i32 404, ; 267
	i32 353, ; 268
	i32 104, ; 269
	i32 341, ; 270
	i32 71, ; 271
	i32 356, ; 272
	i32 352, ; 273
	i32 38, ; 274
	i32 32, ; 275
	i32 103, ; 276
	i32 73, ; 277
	i32 262, ; 278
	i32 410, ; 279
	i32 9, ; 280
	i32 123, ; 281
	i32 46, ; 282
	i32 278, ; 283
	i32 209, ; 284
	i32 379, ; 285
	i32 9, ; 286
	i32 43, ; 287
	i32 4, ; 288
	i32 181, ; 289
	i32 325, ; 290
	i32 192, ; 291
	i32 414, ; 292
	i32 213, ; 293
	i32 233, ; 294
	i32 239, ; 295
	i32 409, ; 296
	i32 367, ; 297
	i32 361, ; 298
	i32 249, ; 299
	i32 31, ; 300
	i32 138, ; 301
	i32 92, ; 302
	i32 93, ; 303
	i32 429, ; 304
	i32 247, ; 305
	i32 49, ; 306
	i32 377, ; 307
	i32 141, ; 308
	i32 355, ; 309
	i32 402, ; 310
	i32 112, ; 311
	i32 140, ; 312
	i32 294, ; 313
	i32 115, ; 314
	i32 342, ; 315
	i32 157, ; 316
	i32 76, ; 317
	i32 79, ; 318
	i32 314, ; 319
	i32 37, ; 320
	i32 336, ; 321
	i32 237, ; 322
	i32 216, ; 323
	i32 298, ; 324
	i32 291, ; 325
	i32 64, ; 326
	i32 138, ; 327
	i32 15, ; 328
	i32 116, ; 329
	i32 330, ; 330
	i32 339, ; 331
	i32 360, ; 332
	i32 286, ; 333
	i32 48, ; 334
	i32 70, ; 335
	i32 80, ; 336
	i32 126, ; 337
	i32 398, ; 338
	i32 192, ; 339
	i32 193, ; 340
	i32 94, ; 341
	i32 121, ; 342
	i32 346, ; 343
	i32 26, ; 344
	i32 189, ; 345
	i32 246, ; 346
	i32 257, ; 347
	i32 307, ; 348
	i32 97, ; 349
	i32 28, ; 350
	i32 282, ; 351
	i32 427, ; 352
	i32 405, ; 353
	i32 149, ; 354
	i32 263, ; 355
	i32 169, ; 356
	i32 369, ; 357
	i32 362, ; 358
	i32 4, ; 359
	i32 401, ; 360
	i32 98, ; 361
	i32 33, ; 362
	i32 93, ; 363
	i32 329, ; 364
	i32 397, ; 365
	i32 205, ; 366
	i32 21, ; 367
	i32 41, ; 368
	i32 170, ; 369
	i32 377, ; 370
	i32 421, ; 371
	i32 300, ; 372
	i32 413, ; 373
	i32 182, ; 374
	i32 314, ; 375
	i32 345, ; 376
	i32 339, ; 377
	i32 319, ; 378
	i32 2, ; 379
	i32 134, ; 380
	i32 111, ; 381
	i32 206, ; 382
	i32 267, ; 383
	i32 433, ; 384
	i32 269, ; 385
	i32 430, ; 386
	i32 58, ; 387
	i32 95, ; 388
	i32 216, ; 389
	i32 412, ; 390
	i32 39, ; 391
	i32 280, ; 392
	i32 196, ; 393
	i32 25, ; 394
	i32 94, ; 395
	i32 89, ; 396
	i32 99, ; 397
	i32 10, ; 398
	i32 179, ; 399
	i32 383, ; 400
	i32 255, ; 401
	i32 356, ; 402
	i32 87, ; 403
	i32 369, ; 404
	i32 100, ; 405
	i32 375, ; 406
	i32 326, ; 407
	i32 199, ; 408
	i32 373, ; 409
	i32 346, ; 410
	i32 259, ; 411
	i32 271, ; 412
	i32 217, ; 413
	i32 384, ; 414
	i32 358, ; 415
	i32 395, ; 416
	i32 409, ; 417
	i32 7, ; 418
	i32 196, ; 419
	i32 311, ; 420
	i32 404, ; 421
	i32 268, ; 422
	i32 88, ; 423
	i32 201, ; 424
	i32 398, ; 425
	i32 306, ; 426
	i32 154, ; 427
	i32 408, ; 428
	i32 33, ; 429
	i32 116, ; 430
	i32 354, ; 431
	i32 82, ; 432
	i32 248, ; 433
	i32 210, ; 434
	i32 20, ; 435
	i32 358, ; 436
	i32 397, ; 437
	i32 11, ; 438
	i32 162, ; 439
	i32 3, ; 440
	i32 229, ; 441
	i32 416, ; 442
	i32 240, ; 443
	i32 241, ; 444
	i32 208, ; 445
	i32 206, ; 446
	i32 84, ; 447
	i32 204, ; 448
	i32 350, ; 449
	i32 64, ; 450
	i32 418, ; 451
	i32 333, ; 452
	i32 143, ; 453
	i32 315, ; 454
	i32 157, ; 455
	i32 193, ; 456
	i32 215, ; 457
	i32 41, ; 458
	i32 117, ; 459
	i32 200, ; 460
	i32 270, ; 461
	i32 412, ; 462
	i32 322, ; 463
	i32 131, ; 464
	i32 359, ; 465
	i32 75, ; 466
	i32 66, ; 467
	i32 422, ; 468
	i32 172, ; 469
	i32 274, ; 470
	i32 143, ; 471
	i32 396, ; 472
	i32 399, ; 473
	i32 106, ; 474
	i32 151, ; 475
	i32 70, ; 476
	i32 250, ; 477
	i32 238, ; 478
	i32 156, ; 479
	i32 214, ; 480
	i32 199, ; 481
	i32 121, ; 482
	i32 127, ; 483
	i32 417, ; 484
	i32 152, ; 485
	i32 297, ; 486
	i32 141, ; 487
	i32 284, ; 488
	i32 414, ; 489
	i32 20, ; 490
	i32 14, ; 491
	i32 135, ; 492
	i32 352, ; 493
	i32 75, ; 494
	i32 59, ; 495
	i32 401, ; 496
	i32 243, ; 497
	i32 287, ; 498
	i32 0, ; 499
	i32 167, ; 500
	i32 168, ; 501
	i32 227, ; 502
	i32 15, ; 503
	i32 74, ; 504
	i32 181, ; 505
	i32 6, ; 506
	i32 175, ; 507
	i32 23, ; 508
	i32 309, ; 509
	i32 261, ; 510
	i32 187, ; 511
	i32 394, ; 512
	i32 251, ; 513
	i32 268, ; 514
	i32 91, ; 515
	i32 219, ; 516
	i32 415, ; 517
	i32 1, ; 518
	i32 136, ; 519
	i32 310, ; 520
	i32 332, ; 521
	i32 399, ; 522
	i32 134, ; 523
	i32 69, ; 524
	i32 146, ; 525
	i32 359, ; 526
	i32 424, ; 527
	i32 367, ; 528
	i32 350, ; 529
	i32 351, ; 530
	i32 301, ; 531
	i32 207, ; 532
	i32 88, ; 533
	i32 96, ; 534
	i32 291, ; 535
	i32 296, ; 536
	i32 419, ; 537
	i32 31, ; 538
	i32 238, ; 539
	i32 264, ; 540
	i32 45, ; 541
	i32 305, ; 542
	i32 195, ; 543
	i32 214, ; 544
	i32 173, ; 545
	i32 375, ; 546
	i32 265, ; 547
	i32 270, ; 548
	i32 109, ; 549
	i32 158, ; 550
	i32 35, ; 551
	i32 22, ; 552
	i32 114, ; 553
	i32 57, ; 554
	i32 330, ; 555
	i32 144, ; 556
	i32 118, ; 557
	i32 120, ; 558
	i32 183, ; 559
	i32 393, ; 560
	i32 110, ; 561
	i32 272, ; 562
	i32 139, ; 563
	i32 278, ; 564
	i32 54, ; 565
	i32 105, ; 566
	i32 425, ; 567
	i32 228, ; 568
	i32 229, ; 569
	i32 133, ; 570
	i32 344, ; 571
	i32 335, ; 572
	i32 323, ; 573
	i32 431, ; 574
	i32 301, ; 575
	i32 231, ; 576
	i32 159, ; 577
	i32 210, ; 578
	i32 410, ; 579
	i32 288, ; 580
	i32 163, ; 581
	i32 373, ; 582
	i32 132, ; 583
	i32 323, ; 584
	i32 386, ; 585
	i32 161, ; 586
	i32 423, ; 587
	i32 312, ; 588
	i32 266, ; 589
	i32 195, ; 590
	i32 140, ; 591
	i32 335, ; 592
	i32 331, ; 593
	i32 169, ; 594
	i32 230, ; 595
	i32 370, ; 596
	i32 265, ; 597
	i32 250, ; 598
	i32 273, ; 599
	i32 340, ; 600
	i32 40, ; 601
	i32 221, ; 602
	i32 299, ; 603
	i32 81, ; 604
	i32 56, ; 605
	i32 37, ; 606
	i32 97, ; 607
	i32 166, ; 608
	i32 172, ; 609
	i32 336, ; 610
	i32 82, ; 611
	i32 275, ; 612
	i32 98, ; 613
	i32 224, ; 614
	i32 30, ; 615
	i32 159, ; 616
	i32 260, ; 617
	i32 18, ; 618
	i32 127, ; 619
	i32 174, ; 620
	i32 119, ; 621
	i32 295, ; 622
	i32 326, ; 623
	i32 191, ; 624
	i32 308, ; 625
	i32 260, ; 626
	i32 328, ; 627
	i32 165, ; 628
	i32 303, ; 629
	i32 180, ; 630
	i32 264, ; 631
	i32 438, ; 632
	i32 325, ; 633
	i32 316, ; 634
	i32 393, ; 635
	i32 170, ; 636
	i32 16, ; 637
	i32 197, ; 638
	i32 222, ; 639
	i32 144, ; 640
	i32 416, ; 641
	i32 400, ; 642
	i32 217, ; 643
	i32 351, ; 644
	i32 125, ; 645
	i32 118, ; 646
	i32 38, ; 647
	i32 115, ; 648
	i32 47, ; 649
	i32 142, ; 650
	i32 117, ; 651
	i32 381, ; 652
	i32 34, ; 653
	i32 252, ; 654
	i32 177, ; 655
	i32 95, ; 656
	i32 53, ; 657
	i32 389, ; 658
	i32 378, ; 659
	i32 218, ; 660
	i32 317, ; 661
	i32 129, ; 662
	i32 153, ; 663
	i32 197, ; 664
	i32 24, ; 665
	i32 161, ; 666
	i32 294, ; 667
	i32 266, ; 668
	i32 148, ; 669
	i32 218, ; 670
	i32 104, ; 671
	i32 366, ; 672
	i32 89, ; 673
	i32 282, ; 674
	i32 60, ; 675
	i32 142, ; 676
	i32 100, ; 677
	i32 5, ; 678
	i32 13, ; 679
	i32 361, ; 680
	i32 242, ; 681
	i32 122, ; 682
	i32 135, ; 683
	i32 28, ; 684
	i32 178, ; 685
	i32 411, ; 686
	i32 213, ; 687
	i32 257, ; 688
	i32 72, ; 689
	i32 292, ; 690
	i32 24, ; 691
	i32 189, ; 692
	i32 280, ; 693
	i32 321, ; 694
	i32 318, ; 695
	i32 428, ; 696
	i32 137, ; 697
	i32 244, ; 698
	i32 273, ; 699
	i32 289, ; 700
	i32 168, ; 701
	i32 363, ; 702
	i32 211, ; 703
	i32 215, ; 704
	i32 322, ; 705
	i32 407, ; 706
	i32 179, ; 707
	i32 382, ; 708
	i32 101, ; 709
	i32 374, ; 710
	i32 123, ; 711
	i32 293, ; 712
	i32 201, ; 713
	i32 202, ; 714
	i32 163, ; 715
	i32 167, ; 716
	i32 296, ; 717
	i32 39, ; 718
	i32 226, ; 719
	i32 224, ; 720
	i32 380, ; 721
	i32 415, ; 722
	i32 232, ; 723
	i32 17, ; 724
	i32 174, ; 725
	i32 171, ; 726
	i32 428, ; 727
	i32 427, ; 728
	i32 137, ; 729
	i32 150, ; 730
	i32 360, ; 731
	i32 285, ; 732
	i32 176, ; 733
	i32 155, ; 734
	i32 130, ; 735
	i32 186, ; 736
	i32 222, ; 737
	i32 389, ; 738
	i32 19, ; 739
	i32 65, ; 740
	i32 255, ; 741
	i32 254, ; 742
	i32 147, ; 743
	i32 173, ; 744
	i32 47, ; 745
	i32 435, ; 746
	i32 271, ; 747
	i32 79, ; 748
	i32 175, ; 749
	i32 61, ; 750
	i32 106, ; 751
	i32 320, ; 752
	i32 275, ; 753
	i32 49, ; 754
	i32 306, ; 755
	i32 432, ; 756
	i32 317, ; 757
	i32 14, ; 758
	i32 176, ; 759
	i32 200, ; 760
	i32 0, ; 761
	i32 68, ; 762
	i32 171, ; 763
	i32 281, ; 764
	i32 285, ; 765
	i32 383, ; 766
	i32 221, ; 767
	i32 437, ; 768
	i32 212, ; 769
	i32 78, ; 770
	i32 371, ; 771
	i32 290, ; 772
	i32 108, ; 773
	i32 274, ; 774
	i32 316, ; 775
	i32 392, ; 776
	i32 67, ; 777
	i32 63, ; 778
	i32 355, ; 779
	i32 27, ; 780
	i32 160, ; 781
	i32 185, ; 782
	i32 248, ; 783
	i32 283, ; 784
	i32 10, ; 785
	i32 226, ; 786
	i32 11, ; 787
	i32 400, ; 788
	i32 78, ; 789
	i32 126, ; 790
	i32 211, ; 791
	i32 83, ; 792
	i32 203, ; 793
	i32 66, ; 794
	i32 107, ; 795
	i32 65, ; 796
	i32 258, ; 797
	i32 128, ; 798
	i32 122, ; 799
	i32 242, ; 800
	i32 77, ; 801
	i32 331, ; 802
	i32 321, ; 803
	i32 436, ; 804
	i32 8, ; 805
	i32 289, ; 806
	i32 259, ; 807
	i32 2, ; 808
	i32 384, ; 809
	i32 44, ; 810
	i32 334, ; 811
	i32 156, ; 812
	i32 128, ; 813
	i32 319, ; 814
	i32 236, ; 815
	i32 23, ; 816
	i32 133, ; 817
	i32 277, ; 818
	i32 308, ; 819
	i32 431, ; 820
	i32 413, ; 821
	i32 371, ; 822
	i32 191, ; 823
	i32 29, ; 824
	i32 385, ; 825
	i32 223, ; 826
	i32 276, ; 827
	i32 263, ; 828
	i32 381, ; 829
	i32 62, ; 830
	i32 184, ; 831
	i32 364, ; 832
	i32 380, ; 833
	i32 228, ; 834
	i32 90, ; 835
	i32 87, ; 836
	i32 148, ; 837
	i32 249, ; 838
	i32 233, ; 839
	i32 230, ; 840
	i32 36, ; 841
	i32 86, ; 842
	i32 297, ; 843
	i32 198, ; 844
	i32 426, ; 845
	i32 366, ; 846
	i32 421, ; 847
	i32 202, ; 848
	i32 50, ; 849
	i32 6, ; 850
	i32 90, ; 851
	i32 433, ; 852
	i32 21, ; 853
	i32 162, ; 854
	i32 188, ; 855
	i32 96, ; 856
	i32 388, ; 857
	i32 50, ; 858
	i32 365, ; 859
	i32 113, ; 860
	i32 313, ; 861
	i32 130, ; 862
	i32 362, ; 863
	i32 183, ; 864
	i32 76, ; 865
	i32 395, ; 866
	i32 27, ; 867
	i32 184, ; 868
	i32 247, ; 869
	i32 290, ; 870
	i32 312, ; 871
	i32 7, ; 872
	i32 262, ; 873
	i32 227, ; 874
	i32 110, ; 875
	i32 313, ; 876
	i32 299 ; 877
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

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
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
