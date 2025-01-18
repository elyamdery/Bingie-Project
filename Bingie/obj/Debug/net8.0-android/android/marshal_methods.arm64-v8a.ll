; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [442 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [878 x i64] [
	i64 24362543149721218, ; 0: Xamarin.AndroidX.DynamicAnimation => 0x568d9a9a43a682 => 296
	i64 54532244727808411, ; 1: Pomelo.EntityFrameworkCore.MySql.dll => 0xc1bcc9a3ce419b => 234
	i64 98382396393917666, ; 2: Microsoft.Extensions.Primitives.dll => 0x15d8644ad360ce2 => 209
	i64 120698629574877762, ; 3: Mono.Android => 0x1accec39cafe242 => 171
	i64 131669012237370309, ; 4: Microsoft.Maui.Essentials.dll => 0x1d3c844de55c3c5 => 230
	i64 182106475126841455, ; 5: Humanizer.dll => 0x286f8dfd13be06f => 178
	i64 196720943101637631, ; 6: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 58
	i64 210515253464952879, ; 7: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 283
	i64 218243443877096319, ; 8: es\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x3075b4182da2b7f => 380
	i64 229794953483747371, ; 9: System.ValueTuple.dll => 0x330654aed93802b => 151
	i64 232391251801502327, ; 10: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 324
	i64 295915112840604065, ; 11: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 327
	i64 316157742385208084, ; 12: Xamarin.AndroidX.Core.Core.Ktx.dll => 0x46337caa7dc1b14 => 290
	i64 350667413455104241, ; 13: System.ServiceProcess.dll => 0x4ddd227954be8f1 => 132
	i64 422779754995088667, ; 14: System.IO.UnmanagedMemoryStream => 0x5de03f27ab57d1b => 56
	i64 435118502366263740, ; 15: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x609d9f8f8bdb9bc => 326
	i64 455940612404211789, ; 16: es\Microsoft.CodeAnalysis.Workspaces.resources => 0x653d3924106f04d => 393
	i64 486223428996552534, ; 17: ZstdSharp.dll => 0x6bf69a1eecfd756 => 351
	i64 517010497809601434, ; 18: Microsoft.IdentityModel.Validators => 0x72cca4efb16d39a => 218
	i64 545109961164950392, ; 19: fi/Microsoft.Maui.Controls.resources.dll => 0x7909e9f1ec38b78 => 411
	i64 560278790331054453, ; 20: System.Reflection.Primitives => 0x7c6829760de3975 => 95
	i64 570522211579385009, ; 21: Serilog.dll => 0x7eae6edbda8d4b1 => 237
	i64 595053104451889001, ; 22: MySql.Data => 0x8420da551592769 => 232
	i64 634256334200181332, ; 23: Microsoft.CodeAnalysis.CSharp.dll => 0x8cd54c6888b1254 => 184
	i64 634308326490598313, ; 24: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 309
	i64 649145001856603771, ; 25: System.Security.SecureString => 0x90239f09b62167b => 129
	i64 668723562677762733, ; 26: Microsoft.Extensions.Configuration.Binder.dll => 0x947c88986577aad => 201
	i64 698487678761130678, ; 27: pt-BR/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x9b186d7d48b5ab6 => 399
	i64 750875890346172408, ; 28: System.Threading.Thread => 0xa6ba5a4da7d1ff8 => 145
	i64 785044228661649374, ; 29: pl/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xae509910e591fde => 398
	i64 798450721097591769, ; 30: Xamarin.AndroidX.Collection.Ktx.dll => 0xb14aab351ad2bd9 => 284
	i64 799765834175365804, ; 31: System.ComponentModel.dll => 0xb1956c9f18442ac => 18
	i64 849051935479314978, ; 32: hi/Microsoft.Maui.Controls.resources.dll => 0xbc8703ca21a3a22 => 414
	i64 870603111519317375, ; 33: SQLitePCLRaw.lib.e_sqlite3.android => 0xc1500ead2756d7f => 246
	i64 872800313462103108, ; 34: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 295
	i64 895210737996778430, ; 35: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0xc6c6d6c5569cbbe => 310
	i64 940822596282819491, ; 36: System.Transactions => 0xd0e792aa81923a3 => 150
	i64 960778385402502048, ; 37: System.Runtime.Handles.dll => 0xd555ed9e1ca1ba0 => 104
	i64 989127641070905171, ; 38: cs\Microsoft.CodeAnalysis.CSharp.resources => 0xdba1659538d2753 => 365
	i64 1010599046655515943, ; 39: System.Reflection.Primitives.dll => 0xe065e7a82401d27 => 95
	i64 1060858978308751610, ; 40: Azure.Core.dll => 0xeb8ed9ebee080fa => 173
	i64 1120440138749646132, ; 41: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 339
	i64 1121665720830085036, ; 42: nb/Microsoft.Maui.Controls.resources.dll => 0xf90f507becf47ac => 422
	i64 1264098730510327121, ; 43: it\Microsoft.CodeAnalysis.Workspaces.resources => 0x118afb1911171d51 => 395
	i64 1268860745194512059, ; 44: System.Drawing.dll => 0x119be62002c19ebb => 36
	i64 1301485588176585670, ; 45: SQLitePCLRaw.core => 0x120fce3f338e43c6 => 244
	i64 1301626418029409250, ; 46: System.Diagnostics.FileVersionInfo => 0x12104e54b4e833e2 => 28
	i64 1315114680217950157, ; 47: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 279
	i64 1369545283391376210, ; 48: Xamarin.AndroidX.Navigation.Fragment.dll => 0x13019a2dd85acb52 => 317
	i64 1404195534211153682, ; 49: System.IO.FileSystem.Watcher.dll => 0x137cb4660bd87f12 => 50
	i64 1425944114962822056, ; 50: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 115
	i64 1476839205573959279, ; 51: System.Net.Primitives.dll => 0x147ec96ece9b1e6f => 70
	i64 1486715745332614827, ; 52: Microsoft.Maui.Controls.dll => 0x14a1e017ea87d6ab => 227
	i64 1492407416628141526, ; 53: it\Microsoft.CodeAnalysis.CSharp.resources => 0x14b618a368475dd6 => 369
	i64 1492954217099365037, ; 54: System.Net.HttpListener => 0x14b809f350210aad => 65
	i64 1513467482682125403, ; 55: Mono.Android.Runtime => 0x1500eaa8245f6c5b => 170
	i64 1518315023656898250, ; 56: SQLitePCLRaw.provider.e_sqlite3 => 0x151223783a354eca => 248
	i64 1537168428375924959, ; 57: System.Threading.Thread.dll => 0x15551e8a954ae0df => 145
	i64 1556147632182429976, ; 58: ko/Microsoft.Maui.Controls.resources.dll => 0x15988c06d24c8918 => 420
	i64 1559087064654078745, ; 59: BCrypt.Net-Next.dll => 0x15a2fd6cc69ce319 => 174
	i64 1576750169145655260, ; 60: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x15e1bdecc376bfdc => 338
	i64 1578461236315596192, ; 61: zh-Hant\Microsoft.CodeAnalysis.resources => 0x15e7d221a250a5a0 => 364
	i64 1624659445732251991, ; 62: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 278
	i64 1628611045998245443, ; 63: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 313
	i64 1636321030536304333, ; 64: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 303
	i64 1651782184287836205, ; 65: System.Globalization.Calendars => 0x16ec4f2524cb982d => 40
	i64 1659106469264733432, ; 66: pt-BR\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x1706548b2113b4f8 => 386
	i64 1659332977923810219, ; 67: System.Reflection.DispatchProxy => 0x1707228d493d63ab => 89
	i64 1672383392659050004, ; 68: Microsoft.Data.Sqlite.dll => 0x17357fd5bfb48e14 => 192
	i64 1682513316613008342, ; 69: System.Net.dll => 0x17597cf276952bd6 => 81
	i64 1718000862390545637, ; 70: ru/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x17d790ae969a6ce5 => 374
	i64 1735388228521408345, ; 71: System.Net.Mail.dll => 0x181556663c69b759 => 66
	i64 1743969030606105336, ; 72: System.Memory.dll => 0x1833d297e88f2af8 => 62
	i64 1767386781656293639, ; 73: System.Private.Uri.dll => 0x188704e9f5582107 => 86
	i64 1769105627832031750, ; 74: Google.Protobuf => 0x188d203205129a06 => 176
	i64 1795316252682057001, ; 75: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 277
	i64 1825687700144851180, ; 76: System.Runtime.InteropServices.RuntimeInformation.dll => 0x1956254a55ef08ec => 106
	i64 1835311033149317475, ; 77: es\Microsoft.Maui.Controls.resources => 0x197855a927386163 => 410
	i64 1836611346387731153, ; 78: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 324
	i64 1854145951182283680, ; 79: System.Runtime.CompilerServices.VisualC => 0x19bb3feb3df2e3a0 => 102
	i64 1865037103900624886, ; 80: Microsoft.Bcl.AsyncInterfaces => 0x19e1f15d56eb87f6 => 182
	i64 1875417405349196092, ; 81: System.Drawing.Primitives => 0x1a06d2319b6c713c => 35
	i64 1875917498431009007, ; 82: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 274
	i64 1881198190668717030, ; 83: tr\Microsoft.Maui.Controls.resources => 0x1a1b5bc992ea9be6 => 432
	i64 1897575647115118287, ; 84: Xamarin.AndroidX.Security.SecurityCrypto => 0x1a558aff4cba86cf => 326
	i64 1920760634179481754, ; 85: Microsoft.Maui.Controls.Xaml => 0x1aa7e99ec2d2709a => 228
	i64 1953472220191506271, ; 86: SQLitePCLRaw.lib.e_sqlcipher.android.dll => 0x1b1c20a2631bbf5f => 245
	i64 1956582621840560024, ; 87: de\Microsoft.CodeAnalysis.CSharp.resources => 0x1b272d8734824f98 => 366
	i64 1959996714666907089, ; 88: tr/Microsoft.Maui.Controls.resources.dll => 0x1b334ea0a2a755d1 => 432
	i64 1966367835830129316, ; 89: fr\Microsoft.CodeAnalysis.Workspaces.resources => 0x1b49f120e06892a4 => 394
	i64 1972384582241139858, ; 90: Microsoft.CodeAnalysis.CSharp => 0x1b5f5153d0f0bc92 => 184
	i64 1972385128188460614, ; 91: System.Security.Cryptography.Algorithms => 0x1b5f51d2edefbe46 => 119
	i64 1981742497975770890, ; 92: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 311
	i64 1983698669889758782, ; 93: cs/Microsoft.Maui.Controls.resources.dll => 0x1b87836e2031a63e => 406
	i64 2019365054093288221, ; 94: Microsoft.CodeAnalysis.VisualBasic.Features.dll => 0x1c0639d15111631d => 189
	i64 2019660174692588140, ; 95: pl/Microsoft.Maui.Controls.resources.dll => 0x1c07463a6f8e1a6c => 424
	i64 2040001226662520565, ; 96: System.Threading.Tasks.Extensions.dll => 0x1c4f8a4ea894a6f5 => 142
	i64 2062890601515140263, ; 97: System.Threading.Tasks.Dataflow => 0x1ca0dc1289cd44a7 => 141
	i64 2064708342624596306, ; 98: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x1ca7514c5eecb152 => 347
	i64 2077488400323790517, ; 99: Microsoft.CodeAnalysis.Features.dll => 0x1cd4b8b16e4b46b5 => 187
	i64 2080945842184875448, ; 100: System.IO.MemoryMappedFiles => 0x1ce10137d8416db8 => 53
	i64 2102659300918482391, ; 101: System.Drawing.Primitives.dll => 0x1d2e257e6aead5d7 => 35
	i64 2106033277907880740, ; 102: System.Threading.Tasks.Dataflow.dll => 0x1d3a221ba6d9cb24 => 141
	i64 2165310824878145998, ; 103: Xamarin.Android.Glide.GifDecoder => 0x1e0cbab9112b81ce => 271
	i64 2165725771938924357, ; 104: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 281
	i64 2192948757939169934, ; 105: Microsoft.EntityFrameworkCore.Abstractions.dll => 0x1e6eeb46cf992a8e => 194
	i64 2200176636225660136, ; 106: Microsoft.Extensions.Logging.Debug.dll => 0x1e8898fe5d5824e8 => 207
	i64 2219986950236918443, ; 107: tr\Microsoft.CodeAnalysis.CSharp.resources => 0x1ecefa5e86dfd2ab => 375
	i64 2262844636196693701, ; 108: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 295
	i64 2287834202362508563, ; 109: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 8
	i64 2287887973817120656, ; 110: System.ComponentModel.DataAnnotations.dll => 0x1fc035fd8d41f790 => 14
	i64 2302323944321350744, ; 111: ru/Microsoft.Maui.Controls.resources.dll => 0x1ff37f6ddb267c58 => 428
	i64 2304837677853103545, ; 112: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0x1ffc6da80d5ed5b9 => 323
	i64 2315304989185124968, ; 113: System.IO.FileSystem.dll => 0x20219d9ee311aa68 => 51
	i64 2316229908869312383, ; 114: Microsoft.IdentityModel.Protocols.OpenIdConnect => 0x2024e6d4884a6f7f => 216
	i64 2323958648452149394, ; 115: cs\Microsoft.CodeAnalysis.resources => 0x20405c13f1aff092 => 352
	i64 2329709569556905518, ; 116: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 306
	i64 2335503487726329082, ; 117: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 136
	i64 2337758774805907496, ; 118: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 101
	i64 2470498323731680442, ; 119: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 288
	i64 2479423007379663237, ; 120: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 333
	i64 2497223385847772520, ; 121: System.Runtime => 0x22a7eb7046413568 => 116
	i64 2547086958574651984, ; 122: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 272
	i64 2592350477072141967, ; 123: System.Xml.dll => 0x23f9e10627330e8f => 163
	i64 2602673633151553063, ; 124: th\Microsoft.Maui.Controls.resources => 0x241e8de13a460e27 => 431
	i64 2612152650457191105, ; 125: Microsoft.IdentityModel.Tokens.dll => 0x24403afeed9892c1 => 217
	i64 2624866290265602282, ; 126: mscorlib.dll => 0x246d65fbde2db8ea => 166
	i64 2632269733008246987, ; 127: System.Net.NameResolution => 0x2487b36034f808cb => 67
	i64 2656907746661064104, ; 128: Microsoft.Extensions.DependencyInjection => 0x24df3b84c8b75da8 => 202
	i64 2662981627730767622, ; 129: cs\Microsoft.Maui.Controls.resources => 0x24f4cfae6c48af06 => 406
	i64 2704260652175460545, ; 130: System.Composition.Convention => 0x258776bc40fc08c1 => 256
	i64 2706075432581334785, ; 131: System.Net.WebSockets => 0x258de944be6c0701 => 80
	i64 2783046991838674048, ; 132: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 101
	i64 2783524070304581857, ; 133: pl/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x26a11064ea4108e1 => 385
	i64 2787234703088983483, ; 134: Xamarin.AndroidX.Startup.StartupRuntime => 0x26ae3f31ef429dbb => 328
	i64 2789714023057451704, ; 135: Microsoft.IdentityModel.JsonWebTokens.dll => 0x26b70e1f9943eab8 => 213
	i64 2815524396660695947, ; 136: System.Security.AccessControl => 0x2712c0857f68238b => 117
	i64 2844780895111324988, ; 137: System.Composition.TypedParts => 0x277ab126dceda53c => 259
	i64 2851879596360956261, ; 138: System.Configuration.ConfigurationManager => 0x2793e9620b477965 => 260
	i64 2895129759130297543, ; 139: fi\Microsoft.Maui.Controls.resources => 0x282d912d479fa4c7 => 411
	i64 2915341129155406726, ; 140: fr/Microsoft.CodeAnalysis.resources.dll => 0x28755f4f9264eb86 => 355
	i64 2923871038697555247, ; 141: Jsr305Binding => 0x2893ad37e69ec52f => 340
	i64 3017136373564924869, ; 142: System.Net.WebProxy => 0x29df058bd93f63c5 => 78
	i64 3017704767998173186, ; 143: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 339
	i64 3063847325783385934, ; 144: System.ClientModel.dll => 0x2a84f8e8eb59674e => 254
	i64 3106852385031680087, ; 145: System.Runtime.Serialization.Xml => 0x2b1dc1c88b637057 => 114
	i64 3110390492489056344, ; 146: System.Security.Cryptography.Csp.dll => 0x2b2a53ac61900058 => 121
	i64 3135773902340015556, ; 147: System.IO.FileSystem.DriveInfo.dll => 0x2b8481c008eac5c4 => 48
	i64 3245892109222566308, ; 148: Humanizer => 0x2d0bb9ad057459a4 => 178
	i64 3257165690781878908, ; 149: Serilog.Sinks.Console => 0x2d33c6f045a4ca7c => 240
	i64 3276991435551191081, ; 150: tr\Microsoft.CodeAnalysis.resources => 0x2d7a36593006b029 => 362
	i64 3281594302220646930, ; 151: System.Security.Principal => 0x2d8a90a198ceba12 => 128
	i64 3289520064315143713, ; 152: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 304
	i64 3303437397778967116, ; 153: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 275
	i64 3311221304742556517, ; 154: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 82
	i64 3325875462027654285, ; 155: System.Runtime.Numerics => 0x2e27e21c8958b48d => 110
	i64 3328853167529574890, ; 156: System.Net.Sockets.dll => 0x2e327651a008c1ea => 75
	i64 3344514922410554693, ; 157: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 350
	i64 3389383627130097293, ; 158: zh-Hant/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x2f0982737e9d4e8d => 403
	i64 3402534845034375023, ; 159: System.IdentityModel.Tokens.Jwt.dll => 0x2f383b6a0629a76f => 262
	i64 3429672777697402584, ; 160: Microsoft.Maui.Essentials => 0x2f98a5385a7b1ed8 => 230
	i64 3437845325506641314, ; 161: System.IO.MemoryMappedFiles.dll => 0x2fb5ae1beb8f7da2 => 53
	i64 3493805808809882663, ; 162: Xamarin.AndroidX.Tracing.Tracing.dll => 0x307c7ddf444f3427 => 330
	i64 3494946837667399002, ; 163: Microsoft.Extensions.Configuration => 0x30808ba1c00a455a => 199
	i64 3508450208084372758, ; 164: System.Net.Ping => 0x30b084e02d03ad16 => 69
	i64 3522470458906976663, ; 165: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 329
	i64 3523004241079211829, ; 166: Microsoft.Extensions.Caching.Memory.dll => 0x30e439b10bb89735 => 198
	i64 3531994851595924923, ; 167: System.Numerics => 0x31042a9aade235bb => 83
	i64 3551103847008531295, ; 168: System.Private.CoreLib.dll => 0x31480e226177735f => 172
	i64 3567343442040498961, ; 169: pt\Microsoft.Maui.Controls.resources => 0x3181bff5bea4ab11 => 426
	i64 3571415421602489686, ; 170: System.Runtime.dll => 0x319037675df7e556 => 116
	i64 3619326080013584352, ; 171: Microsoft.CodeAnalysis.VisualBasic.dll => 0x323a6de4cd8447e0 => 188
	i64 3638003163729360188, ; 172: Microsoft.Extensions.Configuration.Abstractions => 0x327cc89a39d5f53c => 200
	i64 3647754201059316852, ; 173: System.Xml.ReaderWriter => 0x329f6d1e86145474 => 156
	i64 3655542548057982301, ; 174: Microsoft.Extensions.Configuration.dll => 0x32bb18945e52855d => 199
	i64 3659371656528649588, ; 175: Xamarin.Android.Glide.Annotations => 0x32c8b3222885dd74 => 269
	i64 3716579019761409177, ; 176: netstandard.dll => 0x3393f0ed5c8c5c99 => 167
	i64 3727469159507183293, ; 177: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 322
	i64 3772598417116884899, ; 178: Xamarin.AndroidX.DynamicAnimation.dll => 0x345af645b473efa3 => 296
	i64 3869221888984012293, ; 179: Microsoft.Extensions.Logging.dll => 0x35b23cceda0ed605 => 205
	i64 3869649043256705283, ; 180: System.Diagnostics.Tools => 0x35b3c14d74bf0103 => 32
	i64 3890352374528606784, ; 181: Microsoft.Maui.Controls.Xaml.dll => 0x35fd4edf66e00240 => 228
	i64 3919223565570527920, ; 182: System.Security.Cryptography.Encoding => 0x3663e111652bd2b0 => 122
	i64 3933965368022646939, ; 183: System.Net.Requests => 0x369840a8bfadc09b => 72
	i64 3942528141146419395, ; 184: SQLitePCLRaw.lib.e_sqlcipher.android => 0x36b6ac74ba0370c3 => 245
	i64 3966267475168208030, ; 185: System.Memory => 0x370b03412596249e => 62
	i64 3979027889843957915, ; 186: zh-Hant/Microsoft.CodeAnalysis.resources.dll => 0x373858c8b585709b => 364
	i64 3986466921713458903, ; 187: System.Composition.Hosting => 0x3752c68b49935ed7 => 257
	i64 3992675920548082773, ; 188: ru/Microsoft.CodeAnalysis.resources.dll => 0x3768d5987b863455 => 361
	i64 4006972109285359177, ; 189: System.Xml.XmlDocument => 0x379b9fe74ed9fe49 => 161
	i64 4009997192427317104, ; 190: System.Runtime.Serialization.Primitives => 0x37a65f335cf1a770 => 113
	i64 4029369304447093565, ; 191: zh-Hant/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x37eb3208ac6e8b3d => 390
	i64 4073500526318903918, ; 192: System.Private.Xml.dll => 0x3887fb25779ae26e => 88
	i64 4073631083018132676, ; 193: Microsoft.Maui.Controls.Compatibility.dll => 0x388871e311491cc4 => 226
	i64 4120493066591692148, ; 194: zh-Hant\Microsoft.Maui.Controls.resources => 0x392eee9cdda86574 => 437
	i64 4135615024468428857, ; 195: Syncfusion.Maui.Popup => 0x3964a7f40d358839 => 253
	i64 4148881117810174540, ; 196: System.Runtime.InteropServices.JavaScript.dll => 0x3993c9651a66aa4c => 105
	i64 4154383907710350974, ; 197: System.ComponentModel => 0x39a7562737acb67e => 18
	i64 4167269041631776580, ; 198: System.Threading.ThreadPool => 0x39d51d1d3df1cf44 => 146
	i64 4168469861834746866, ; 199: System.Security.Claims.dll => 0x39d96140fb94ebf2 => 118
	i64 4187479170553454871, ; 200: System.Linq.Expressions => 0x3a1cea1e912fa117 => 58
	i64 4201423742386704971, ; 201: Xamarin.AndroidX.Core.Core.Ktx => 0x3a4e74a233da124b => 290
	i64 4205801962323029395, ; 202: System.ComponentModel.TypeConverter => 0x3a5e0299f7e7ad93 => 17
	i64 4235503420553921860, ; 203: System.IO.IsolatedStorage.dll => 0x3ac787eb9b118544 => 52
	i64 4282138915307457788, ; 204: System.Reflection.Emit => 0x3b6d36a7ddc70cfc => 92
	i64 4303454211501947484, ; 205: ru/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x3bb8f0cdffbb7a5c => 400
	i64 4321865999928413850, ; 206: System.Diagnostics.EventLog.dll => 0x3bfa5a3a8c924e9a => 261
	i64 4337444564132831293, ; 207: SQLitePCLRaw.batteries_v2.dll => 0x3c31b2d9ae16203d => 243
	i64 4356591372459378815, ; 208: vi/Microsoft.Maui.Controls.resources.dll => 0x3c75b8c562f9087f => 434
	i64 4371205382599439717, ; 209: cs\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x3ca9a422c61d7565 => 378
	i64 4373617458794931033, ; 210: System.IO.Pipes.dll => 0x3cb235e806eb2359 => 55
	i64 4397634830160618470, ; 211: System.Security.SecureString.dll => 0x3d0789940f9be3e6 => 129
	i64 4477672992252076438, ; 212: System.Web.HttpUtility.dll => 0x3e23e3dcdb8ba196 => 152
	i64 4484706122338676047, ; 213: System.Globalization.Extensions.dll => 0x3e3ce07510042d4f => 41
	i64 4513320955448359355, ; 214: Microsoft.EntityFrameworkCore.Relational => 0x3ea2897f12d379bb => 195
	i64 4533124835995628778, ; 215: System.Reflection.Emit.dll => 0x3ee8e505540534ea => 92
	i64 4575529722446359306, ; 216: zh-Hans/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x3f7f8c0a55a82b0a => 402
	i64 4612482779465751747, ; 217: Microsoft.EntityFrameworkCore.Abstractions => 0x4002d4a662a99cc3 => 194
	i64 4633188143799146779, ; 218: fr\Microsoft.CodeAnalysis.resources => 0x404c6411b0b3191b => 355
	i64 4636684751163556186, ; 219: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 334
	i64 4672453897036726049, ; 220: System.IO.FileSystem.Watcher => 0x40d7e4104a437f21 => 50
	i64 4679594760078841447, ; 221: ar/Microsoft.Maui.Controls.resources.dll => 0x40f142a407475667 => 404
	i64 4716677666592453464, ; 222: System.Xml.XmlSerializer => 0x417501590542f358 => 162
	i64 4743821336939966868, ; 223: System.ComponentModel.Annotations => 0x41d5705f4239b194 => 13
	i64 4759461199762736555, ; 224: Xamarin.AndroidX.Lifecycle.Process.dll => 0x420d00be961cc5ab => 308
	i64 4766253714849837372, ; 225: pt-BR/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x422522803f02b13c => 386
	i64 4794310189461587505, ; 226: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 272
	i64 4795410492532947900, ; 227: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 329
	i64 4809057822547766521, ; 228: System.Drawing => 0x42bd349c3145ecf9 => 36
	i64 4814660307502931973, ; 229: System.Net.NameResolution.dll => 0x42d11c0a5ee2a005 => 67
	i64 4853321196694829351, ; 230: System.Runtime.Loader.dll => 0x435a75ea15de7927 => 109
	i64 4855769773892993059, ; 231: cs/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x436328e1e3f88023 => 378
	i64 4945597469301700026, ; 232: ja/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x44a24ab207cc75ba => 383
	i64 5024407331517470841, ; 233: Microsoft.Kiota.Http.HttpClientLibrary.dll => 0x45ba47d8f9f79879 => 221
	i64 5032256205035195147, ; 234: MySql.Data.dll => 0x45d62a5b3fe0cb0b => 232
	i64 5055365687667823624, ; 235: Xamarin.AndroidX.Activity.Ktx.dll => 0x4628444ef7239408 => 273
	i64 5060040782518347251, ; 236: Microsoft.Kiota.Serialization.Multipart => 0x4638e0484efee5f3 => 224
	i64 5081566143765835342, ; 237: System.Resources.ResourceManager.dll => 0x4685597c05d06e4e => 99
	i64 5083120864858317402, ; 238: zh-Hans\Microsoft.CodeAnalysis.resources => 0x468adf7ebc41a25a => 363
	i64 5099468265966638712, ; 239: System.Resources.ResourceManager => 0x46c4f35ea8519678 => 99
	i64 5101282090298625898, ; 240: ja\Microsoft.CodeAnalysis.Workspaces.resources => 0x46cb65088b4fcf6a => 396
	i64 5103417709280584325, ; 241: System.Collections.Specialized => 0x46d2fb5e161b6285 => 11
	i64 5107702058248948463, ; 242: ru\Microsoft.CodeAnalysis.CSharp.resources => 0x46e233f5d075caef => 374
	i64 5129462924058778861, ; 243: Microsoft.Data.Sqlite => 0x472f835a350f5ced => 192
	i64 5182934613077526976, ; 244: System.Collections.Specialized.dll => 0x47ed7b91fa9009c0 => 11
	i64 5188556352435445191, ; 245: Microsoft.Kiota.Authentication.Azure.dll => 0x480174832c02ddc7 => 220
	i64 5205316157927637098, ; 246: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 315
	i64 5244375036463807528, ; 247: System.Diagnostics.Contracts.dll => 0x48c7c34f4d59fc28 => 25
	i64 5262971552273843408, ; 248: System.Security.Principal.dll => 0x4909d4be0c44c4d0 => 128
	i64 5278787618751394462, ; 249: System.Net.WebClient.dll => 0x4942055efc68329e => 76
	i64 5280980186044710147, ; 250: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x4949cf7fd7123d03 => 307
	i64 5290786973231294105, ; 251: System.Runtime.Loader => 0x496ca6b869b72699 => 109
	i64 5332349484191854038, ; 252: Syncfusion.Maui.Core.dll => 0x4a004f9a977e2dd6 => 252
	i64 5376510917114486089, ; 253: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 333
	i64 5408338804355907810, ; 254: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 331
	i64 5423376490970181369, ; 255: System.Runtime.InteropServices.RuntimeInformation => 0x4b43b42f2b7b6ef9 => 106
	i64 5440320908473006344, ; 256: Microsoft.VisualBasic.Core => 0x4b7fe70acda9f908 => 2
	i64 5446034149219586269, ; 257: System.Diagnostics.Debug => 0x4b94333452e150dd => 26
	i64 5451019430259338467, ; 258: Xamarin.AndroidX.ConstraintLayout.dll => 0x4ba5e94a845c2ce3 => 286
	i64 5457765010617926378, ; 259: System.Xml.Serialization => 0x4bbde05c557002ea => 157
	i64 5462284562947176812, ; 260: Microsoft.Kiota.Serialization.Text => 0x4bcdeede9c90f96c => 225
	i64 5471532531798518949, ; 261: sv\Microsoft.Maui.Controls.resources => 0x4beec9d926d82ca5 => 430
	i64 5488847537322884930, ; 262: System.Windows.Extensions => 0x4c2c4dc108687f42 => 267
	i64 5507995362134886206, ; 263: System.Core.dll => 0x4c705499688c873e => 21
	i64 5522859530602327440, ; 264: uk\Microsoft.Maui.Controls.resources => 0x4ca5237b51eead90 => 433
	i64 5527431512186326818, ; 265: System.IO.FileSystem.Primitives.dll => 0x4cb561acbc2a8f22 => 49
	i64 5570799893513421663, ; 266: System.IO.Compression.Brotli => 0x4d4f74fcdfa6c35f => 43
	i64 5573260873512690141, ; 267: System.Security.Cryptography.dll => 0x4d58333c6e4ea1dd => 126
	i64 5574231584441077149, ; 268: Xamarin.AndroidX.Annotation.Jvm => 0x4d5ba617ae5f8d9d => 276
	i64 5586474322064658720, ; 269: fr\Microsoft.CodeAnalysis.CSharp.resources => 0x4d8724cc29815120 => 368
	i64 5591791169662171124, ; 270: System.Linq.Parallel => 0x4d9a087135e137f4 => 59
	i64 5650097808083101034, ; 271: System.Security.Cryptography.Algorithms.dll => 0x4e692e055d01a56a => 119
	i64 5692067934154308417, ; 272: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 336
	i64 5697338526674305454, ; 273: pl\Microsoft.CodeAnalysis.CSharp.resources => 0x4f1103344791c1ae => 372
	i64 5724799082821825042, ; 274: Xamarin.AndroidX.ExifInterface => 0x4f72926f3e13b212 => 299
	i64 5757522595884336624, ; 275: Xamarin.AndroidX.Concurrent.Futures.dll => 0x4fe6d44bd9f885f0 => 285
	i64 5783556987928984683, ; 276: Microsoft.VisualBasic => 0x504352701bbc3c6b => 3
	i64 5812387745074149618, ; 277: K4os.Compression.LZ4.dll => 0x50a9bfdbd9fa78f2 => 179
	i64 5896680224035167651, ; 278: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 305
	i64 5959344983920014087, ; 279: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x52b3d8b05c8ef307 => 325
	i64 5979151488806146654, ; 280: System.Formats.Asn1 => 0x52fa3699a489d25e => 38
	i64 5981100626307227755, ; 281: pt-BR\Microsoft.CodeAnalysis.CSharp.resources => 0x5301235494e8a06b => 373
	i64 5984759512290286505, ; 282: System.Security.Cryptography.Primitives => 0x530e23115c33dba9 => 124
	i64 6052006988162547083, ; 283: ja\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x53fd0c4a739c3d8b => 383
	i64 6052904953572895746, ; 284: tr\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x54003cfc50ea2c02 => 388
	i64 6068057819846744445, ; 285: ro/Microsoft.Maui.Controls.resources.dll => 0x5436126fec7f197d => 427
	i64 6102788177522843259, ; 286: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0x54b1758374b3de7b => 325
	i64 6167632067760390963, ; 287: ja/Microsoft.CodeAnalysis.resources.dll => 0x5597d4b0282a8333 => 357
	i64 6183170893902868313, ; 288: SQLitePCLRaw.batteries_v2 => 0x55cf092b0c9d6f59 => 243
	i64 6200764641006662125, ; 289: ro\Microsoft.Maui.Controls.resources => 0x560d8a96830131ed => 427
	i64 6222399776351216807, ; 290: System.Text.Json.dll => 0x565a67a0ffe264a7 => 137
	i64 6251069312384999852, ; 291: System.Transactions.Local => 0x56c0426b870da1ac => 149
	i64 6278736998281604212, ; 292: System.Private.DataContractSerialization => 0x57228e08a4ad6c74 => 85
	i64 6284145129771520194, ; 293: System.Reflection.Emit.ILGeneration => 0x5735c4b3610850c2 => 90
	i64 6296727896078076854, ; 294: ja/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x576278a8f506cbb6 => 370
	i64 6319713645133255417, ; 295: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 309
	i64 6357457916754632952, ; 296: _Microsoft.Android.Resource.Designer => 0x583a3a4ac2a7a0f8 => 438
	i64 6363738694785491292, ; 297: es/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x58508aa05a18e15c => 380
	i64 6397768165450447711, ; 298: es\Microsoft.CodeAnalysis.CSharp.resources => 0x58c9703fe8f9fb5f => 367
	i64 6401687960814735282, ; 299: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 306
	i64 6405571018036478432, ; 300: ko\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x58e528e7199a01e0 => 384
	i64 6478287442656530074, ; 301: hr\Microsoft.Maui.Controls.resources => 0x59e7801b0c6a8e9a => 415
	i64 6504860066809920875, ; 302: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 281
	i64 6548213210057960872, ; 303: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 292
	i64 6557084851308642443, ; 304: Xamarin.AndroidX.Window.dll => 0x5aff71ee6c58c08b => 337
	i64 6560151584539558821, ; 305: Microsoft.Extensions.Options => 0x5b0a571be53243a5 => 208
	i64 6589202984700901502, ; 306: Xamarin.Google.ErrorProne.Annotations.dll => 0x5b718d34180a787e => 342
	i64 6591971792923354531, ; 307: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x5b7b636b7e9765a3 => 307
	i64 6617685658146568858, ; 308: System.Text.Encoding.CodePages => 0x5bd6be0b4905fa9a => 133
	i64 6642279005832796386, ; 309: tr\Microsoft.CodeAnalysis.Workspaces.resources => 0x5c2e1d9041b2d8e2 => 401
	i64 6713440830605852118, ; 310: System.Reflection.TypeExtensions.dll => 0x5d2aeeddb8dd7dd6 => 96
	i64 6739853162153639747, ; 311: Microsoft.VisualBasic.dll => 0x5d88c4bde075ff43 => 3
	i64 6743165466166707109, ; 312: nl\Microsoft.Maui.Controls.resources => 0x5d948943c08c43a5 => 423
	i64 6772837112740759457, ; 313: System.Runtime.InteropServices.JavaScript => 0x5dfdf378527ec7a1 => 105
	i64 6777482997383978746, ; 314: pt/Microsoft.Maui.Controls.resources.dll => 0x5e0e74e0a2525efa => 426
	i64 6786606130239981554, ; 315: System.Diagnostics.TraceSource => 0x5e2ede51877147f2 => 33
	i64 6798329586179154312, ; 316: System.Windows => 0x5e5884bd523ca188 => 154
	i64 6800157191326250309, ; 317: Microsoft.CodeAnalysis.CSharp.Workspaces => 0x5e5f02efcdd1b545 => 186
	i64 6814185388980153342, ; 318: System.Xml.XDocument.dll => 0x5e90d98217d1abfe => 158
	i64 6876862101832370452, ; 319: System.Xml.Linq => 0x5f6f85a57d108914 => 155
	i64 6881674271711615136, ; 320: pl\Microsoft.CodeAnalysis.Workspaces.resources => 0x5f809e4a1950d8a0 => 398
	i64 6894844156784520562, ; 321: System.Numerics.Vectors => 0x5faf683aead1ad72 => 82
	i64 6916425539059316312, ; 322: de\Microsoft.CodeAnalysis.resources => 0x5ffc14620b11f658 => 353
	i64 7011053663211085209, ; 323: Xamarin.AndroidX.Fragment.Ktx => 0x614c442918e5dd99 => 301
	i64 7060896174307865760, ; 324: System.Threading.Tasks.Parallel.dll => 0x61fd57a90988f4a0 => 143
	i64 7083547580668757502, ; 325: System.Private.Xml.Linq.dll => 0x624dd0fe8f56c5fe => 87
	i64 7101497697220435230, ; 326: System.Configuration => 0x628d9687c0141d1e => 19
	i64 7103753931438454322, ; 327: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 302
	i64 7112547816752919026, ; 328: System.IO.FileSystem => 0x62b4d88e3189b1f2 => 51
	i64 7174194263796106178, ; 329: Microsoft.Kiota.Http.HttpClientLibrary => 0x638fdbac233643c2 => 221
	i64 7188876148444261747, ; 330: System.Composition.AttributedModel.dll => 0x63c404c4ca4c6d73 => 255
	i64 7192745174564810625, ; 331: Xamarin.Android.Glide.GifDecoder.dll => 0x63d1c3a0a1d72f81 => 271
	i64 7219616639871433054, ; 332: ja\Microsoft.CodeAnalysis.CSharp.resources => 0x64313b153209395e => 370
	i64 7220009545223068405, ; 333: sv/Microsoft.Maui.Controls.resources.dll => 0x6432a06d99f35af5 => 430
	i64 7270811800166795866, ; 334: System.Linq => 0x64e71ccf51a90a5a => 61
	i64 7299370801165188114, ; 335: System.IO.Pipes.AccessControl.dll => 0x654c9311e74f3c12 => 54
	i64 7316205155833392065, ; 336: Microsoft.Win32.Primitives => 0x658861d38954abc1 => 4
	i64 7338192458477945005, ; 337: System.Reflection => 0x65d67f295d0740ad => 97
	i64 7348123982286201829, ; 338: System.Memory.Data.dll => 0x65f9c7d471b2a3e5 => 264
	i64 7349431895026339542, ; 339: Xamarin.Android.Glide.DiskLruCache => 0x65fe6d5e9bf88ed6 => 270
	i64 7377312882064240630, ; 340: System.ComponentModel.TypeConverter.dll => 0x66617afac45a2ff6 => 17
	i64 7412872140774854801, ; 341: pt-BR/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x66dfcfefdc465091 => 373
	i64 7451202609009583483, ; 342: K4os.Hash.xxHash => 0x6767fd4b737ae57b => 181
	i64 7488575175965059935, ; 343: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 155
	i64 7489048572193775167, ; 344: System.ObjectModel => 0x67ee71ff6b419e3f => 84
	i64 7496222613193209122, ; 345: System.IdentityModel.Tokens.Jwt => 0x6807eec000a1b522 => 262
	i64 7592577537120840276, ; 346: System.Diagnostics.Process => 0x695e410af5b2aa54 => 29
	i64 7637303409920963731, ; 347: System.IO.Compression.ZipFile.dll => 0x69fd26fcb637f493 => 45
	i64 7654504624184590948, ; 348: System.Net.Http => 0x6a3a4366801b8264 => 64
	i64 7694700312542370399, ; 349: System.Net.Mail => 0x6ac9112a7e2cda5f => 66
	i64 7702918024138448955, ; 350: MySqlConnector => 0x6ae6432192b9e03b => 233
	i64 7708790323521193081, ; 351: ms/Microsoft.Maui.Controls.resources.dll => 0x6afb1ff4d1730479 => 421
	i64 7714652370974252055, ; 352: System.Private.CoreLib => 0x6b0ff375198b9c17 => 172
	i64 7725404731275645577, ; 353: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x6b3626ac11ce9289 => 310
	i64 7735176074855944702, ; 354: Microsoft.CSharp => 0x6b58dda848e391fe => 1
	i64 7735352534559001595, ; 355: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 345
	i64 7769135412902976898, ; 356: Syncfusion.Maui.Popup.dll => 0x6bd1837ed1fd5d82 => 253
	i64 7791074099216502080, ; 357: System.IO.FileSystem.AccessControl.dll => 0x6c1f749d468bcd40 => 47
	i64 7811495074685974230, ; 358: Microsoft.Graph.dll => 0x6c680162235ef6d6 => 210
	i64 7820441508502274321, ; 359: System.Data => 0x6c87ca1e14ff8111 => 24
	i64 7836164640616011524, ; 360: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 278
	i64 7872210730649581996, ; 361: de/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x6d3fb5f36562f9ac => 366
	i64 7877653024023762272, ; 362: Microsoft.CodeAnalysis.CSharp.Features => 0x6d530bb010b7d560 => 185
	i64 7880754438529995359, ; 363: ko/Microsoft.CodeAnalysis.resources.dll => 0x6d5e106866a9d25f => 358
	i64 7929771293765145963, ; 364: pl\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x6e0c34fa5af6156b => 385
	i64 7970996770859424023, ; 365: Microsoft.Kiota.Serialization.Form.dll => 0x6e9eab54b8dc9917 => 222
	i64 7972383140441761405, ; 366: Microsoft.Extensions.Caching.Abstractions.dll => 0x6ea3983a0b58267d => 197
	i64 8004205632400797805, ; 367: de/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x6f14a69d546e306d => 379
	i64 8004654074791351515, ; 368: ko\Microsoft.CodeAnalysis.Workspaces.resources => 0x6f163e7875d254db => 397
	i64 8025517457475554965, ; 369: WindowsBase => 0x6f605d9b4786ce95 => 165
	i64 8031450141206250471, ; 370: System.Runtime.Intrinsics.dll => 0x6f757159d9dc03e7 => 108
	i64 8040751982668687859, ; 371: Microsoft.Kiota.Serialization.Text.dll => 0x6f967d5395fc29f3 => 225
	i64 8064050204834738623, ; 372: System.Collections.dll => 0x6fe942efa61731bf => 12
	i64 8069053972940814620, ; 373: ko/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x6ffb09d63297791c => 397
	i64 8083354569033831015, ; 374: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 304
	i64 8085230611270010360, ; 375: System.Net.Http.Json.dll => 0x703482674fdd05f8 => 63
	i64 8087206902342787202, ; 376: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 27
	i64 8103644804370223335, ; 377: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 23
	i64 8113615946733131500, ; 378: System.Reflection.Extensions => 0x70995ab73cf916ec => 93
	i64 8129655575090457984, ; 379: de\Microsoft.CodeAnalysis.Workspaces.resources => 0x70d256ac3b8a6d80 => 392
	i64 8167236081217502503, ; 380: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 168
	i64 8185542183669246576, ; 381: System.Collections => 0x7198e33f4794aa70 => 12
	i64 8187640529827139739, ; 382: Xamarin.KotlinX.Coroutines.Android => 0x71a057ae90f0109b => 349
	i64 8192634701712173889, ; 383: Serilog.Settings.Configuration.dll => 0x71b215dad217ab41 => 239
	i64 8198475655723014119, ; 384: Microsoft.CodeAnalysis.CSharp.Features.dll => 0x71c6d62be70ed7e7 => 185
	i64 8246048515196606205, ; 385: Microsoft.Maui.Graphics.dll => 0x726fd96f64ee56fd => 231
	i64 8264926008854159966, ; 386: System.Diagnostics.Process.dll => 0x72b2ea6a64a3a25e => 29
	i64 8290740647658429042, ; 387: System.Runtime.Extensions => 0x730ea0b15c929a72 => 103
	i64 8318905602908530212, ; 388: System.ComponentModel.DataAnnotations => 0x7372b092055ea624 => 14
	i64 8368397830801818644, ; 389: Std.UriTemplate.dll => 0x7422857d4c178414 => 249
	i64 8368701292315763008, ; 390: System.Security.Cryptography => 0x7423997c6fd56140 => 126
	i64 8398329775253868912, ; 391: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x748cdc6f3097d170 => 287
	i64 8400357532724379117, ; 392: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 319
	i64 8410671156615598628, ; 393: System.Reflection.Emit.Lightweight.dll => 0x74b8b4daf4b25224 => 91
	i64 8426919725312979251, ; 394: Xamarin.AndroidX.Lifecycle.Process => 0x74f26ed7aa033133 => 308
	i64 8446909045495485438, ; 395: Microsoft.CodeAnalysis.VisualBasic.Features => 0x75397305f2cad3fe => 189
	i64 8452111768915975231, ; 396: ko/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x754beedf64229c3f => 371
	i64 8476828615142258695, ; 397: BCrypt.Net-Next => 0x75a3beb69b6bb807 => 174
	i64 8476857680833348370, ; 398: System.Security.Permissions.dll => 0x75a3d925fd9d0312 => 266
	i64 8518412311883997971, ; 399: System.Collections.Immutable => 0x76377add7c28e313 => 9
	i64 8538413690921358003, ; 400: tr/Microsoft.CodeAnalysis.resources.dll => 0x767e8a0370b312b3 => 362
	i64 8563666267364444763, ; 401: System.Private.Uri => 0x76d841191140ca5b => 86
	i64 8595707132524137149, ; 402: cs/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x774a161853648abd => 365
	i64 8598790081731763592, ; 403: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x77550a055fc61d88 => 298
	i64 8601935802264776013, ; 404: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 331
	i64 8614108721271900878, ; 405: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x778b763e14018ace => 425
	i64 8623059219396073920, ; 406: System.Net.Quic.dll => 0x77ab42ac514299c0 => 71
	i64 8626175481042262068, ; 407: Java.Interop => 0x77b654e585b55834 => 168
	i64 8638972117149407195, ; 408: Microsoft.CSharp.dll => 0x77e3cb5e8b31d7db => 1
	i64 8639588376636138208, ; 409: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 318
	i64 8648495978913578441, ; 410: Microsoft.Win32.Registry.dll => 0x7805a1456889bdc9 => 5
	i64 8677882282824630478, ; 411: pt-BR\Microsoft.Maui.Controls.resources => 0x786e07f5766b00ce => 425
	i64 8684531736582871431, ; 412: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 44
	i64 8725526185868997716, ; 413: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 27
	i64 8747977504141423047, ; 414: zh-Hans\Microsoft.CodeAnalysis.CSharp.resources => 0x79670f30f57531c7 => 376
	i64 8784689560710160013, ; 415: zh-Hant\Microsoft.CodeAnalysis.Workspaces.resources => 0x79e97c9cb8361e8d => 403
	i64 8853378295825400934, ; 416: Xamarin.Kotlin.StdLib.Common.dll => 0x7add84a720d38466 => 346
	i64 8932705878791833663, ; 417: pt-BR\Microsoft.CodeAnalysis.Workspaces.resources => 0x7bf758ab546e483f => 399
	i64 8941376889969657626, ; 418: System.Xml.XDocument => 0x7c1626e87187471a => 158
	i64 8951477988056063522, ; 419: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0x7c3a09cd9ccf5e22 => 321
	i64 8954753533646919997, ; 420: System.Runtime.Serialization.Json => 0x7c45ace50032d93d => 112
	i64 9045785047181495996, ; 421: zh-HK\Microsoft.Maui.Controls.resources => 0x7d891592e3cb0ebc => 435
	i64 9052662452269567435, ; 422: Microsoft.IdentityModel.Protocols => 0x7da184898b0b4dcb => 215
	i64 9111603110219107042, ; 423: Microsoft.Extensions.Caching.Memory => 0x7e72eac0def44ae2 => 198
	i64 9138683372487561558, ; 424: System.Security.Cryptography.Csp => 0x7ed3201bc3e3d156 => 121
	i64 9146833000203878303, ; 425: pl/Microsoft.CodeAnalysis.resources.dll => 0x7ef01426d4f8ff9f => 359
	i64 9165872221346508209, ; 426: es/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x7f33b838f1faadb1 => 367
	i64 9250544137016314866, ; 427: Microsoft.EntityFrameworkCore => 0x806088e191ee0bf2 => 193
	i64 9258038534689720297, ; 428: Microsoft.CodeAnalysis.Workspaces => 0x807b28ff010f07e9 => 191
	i64 9286073997824813334, ; 429: BouncyCastle.Cryptography => 0x80dec319ee56e916 => 175
	i64 9312692141327339315, ; 430: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 336
	i64 9324707631942237306, ; 431: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 277
	i64 9387065344160869883, ; 432: SQLitePCLRaw.provider.e_sqlcipher => 0x82458e321a16adfb => 247
	i64 9427266486299436557, ; 433: Microsoft.IdentityModel.Logging.dll => 0x82d460ebe6d2a60d => 214
	i64 9468215723722196442, ; 434: System.Xml.XPath.XDocument.dll => 0x8365dc09353ac5da => 159
	i64 9554839972845591462, ; 435: System.ServiceModel.Web => 0x84999c54e32a1ba6 => 131
	i64 9575902398040817096, ; 436: Xamarin.Google.Crypto.Tink.Android.dll => 0x84e4707ee708bdc8 => 341
	i64 9584643793929893533, ; 437: System.IO.dll => 0x85037ebfbbd7f69d => 57
	i64 9640812368965969847, ; 438: it/Microsoft.CodeAnalysis.resources.dll => 0x85cb0bc53668a7b7 => 356
	i64 9659729154652888475, ; 439: System.Text.RegularExpressions => 0x860e407c9991dd9b => 138
	i64 9662334977499516867, ; 440: System.Numerics.dll => 0x8617827802b0cfc3 => 83
	i64 9667360217193089419, ; 441: System.Diagnostics.StackTrace => 0x86295ce5cd89898b => 30
	i64 9678050649315576968, ; 442: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 288
	i64 9702891218465930390, ; 443: System.Collections.NonGeneric.dll => 0x86a79827b2eb3c96 => 10
	i64 9720196544438918509, ; 444: Microsoft.CodeAnalysis.VisualBasic.Workspaces.dll => 0x86e51341e242dd6d => 190
	i64 9737654085557355179, ; 445: Serilog => 0x872318cc6b4702ab => 237
	i64 9776157902332307015, ; 446: de/Microsoft.CodeAnalysis.resources.dll => 0x87abe3d0dca52647 => 353
	i64 9780093022148426479, ; 447: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x87b9dec9576efaef => 338
	i64 9808709177481450983, ; 448: Mono.Android.dll => 0x881f890734e555e7 => 171
	i64 9815966120248698980, ; 449: it\Microsoft.CodeAnalysis.resources => 0x8839512ddcb16864 => 356
	i64 9819168441846169364, ; 450: Microsoft.IdentityModel.Protocols.dll => 0x8844b1ac75f77f14 => 215
	i64 9825649861376906464, ; 451: Xamarin.AndroidX.Concurrent.Futures => 0x885bb87d8abc94e0 => 285
	i64 9834056768316610435, ; 452: System.Transactions.dll => 0x8879968718899783 => 150
	i64 9836529246295212050, ; 453: System.Reflection.Metadata => 0x88825f3bbc2ac012 => 94
	i64 9864956466380592553, ; 454: Microsoft.EntityFrameworkCore.Sqlite => 0x88e75da3af4ed5a9 => 196
	i64 9907349773706910547, ; 455: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x897dfa20b758db53 => 298
	i64 9926151752036674810, ; 456: Serilog.Extensions.Logging.dll => 0x89c0c66d6ec46cfa => 238
	i64 9933555792566666578, ; 457: System.Linq.Queryable.dll => 0x89db145cf475c552 => 60
	i64 9934643601996120594, ; 458: it/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x89def1b853160212 => 395
	i64 9956195530459977388, ; 459: Microsoft.Maui => 0x8a2b8315b36616ac => 229
	i64 9969399256958809458, ; 460: de/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x8a5a6bcdd712b572 => 392
	i64 9974604633896246661, ; 461: System.Xml.Serialization.dll => 0x8a6cea111a59dd85 => 157
	i64 9991543690424095600, ; 462: es/Microsoft.Maui.Controls.resources.dll => 0x8aa9180c89861370 => 410
	i64 9998685624638532270, ; 463: K4os.Hash.xxHash.dll => 0x8ac27799ad626aae => 181
	i64 10017511394021241210, ; 464: Microsoft.Extensions.Logging.Debug => 0x8b055989ae10717a => 207
	i64 10038780035334861115, ; 465: System.Net.Http.dll => 0x8b50e941206af13b => 64
	i64 10049750028500509718, ; 466: cs\Microsoft.CodeAnalysis.Workspaces.resources => 0x8b77e267b23ea416 => 391
	i64 10051358222726253779, ; 467: System.Private.Xml => 0x8b7d990c97ccccd3 => 88
	i64 10078727084704864206, ; 468: System.Net.WebSockets.Client => 0x8bded4e257f117ce => 79
	i64 10089571585547156312, ; 469: System.IO.FileSystem.AccessControl => 0x8c055be67469bb58 => 47
	i64 10092835686693276772, ; 470: Microsoft.Maui.Controls => 0x8c10f49539bd0c64 => 227
	i64 10099987355287021298, ; 471: Microsoft.Kiota.Serialization.Json => 0x8c2a5cfcd3d1e6f2 => 223
	i64 10105485790837105934, ; 472: System.Threading.Tasks.Parallel => 0x8c3de5c91d9a650e => 143
	i64 10143853363526200146, ; 473: da\Microsoft.Maui.Controls.resources => 0x8cc634e3c2a16b52 => 407
	i64 10205853378024263619, ; 474: Microsoft.Extensions.Configuration.Binder => 0x8da279930adb4fc3 => 201
	i64 10226222362177979215, ; 475: Xamarin.Kotlin.StdLib.Jdk7 => 0x8dead70ebbc6434f => 347
	i64 10229024438826829339, ; 476: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 292
	i64 10236703004850800690, ; 477: System.Net.ServicePoint.dll => 0x8e101325834e4832 => 74
	i64 10245369515835430794, ; 478: System.Reflection.Emit.Lightweight => 0x8e2edd4ad7fc978a => 91
	i64 10321854143672141184, ; 479: Xamarin.Jetbrains.Annotations.dll => 0x8f3e97a7f8f8c580 => 344
	i64 10347389959537838075, ; 480: ru\Microsoft.CodeAnalysis.resources => 0x8f9950586ab247fb => 361
	i64 10356409254419335618, ; 481: ru\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x8fb95b58a63b89c2 => 387
	i64 10360651442923773544, ; 482: System.Text.Encoding => 0x8fc86d98211c1e68 => 135
	i64 10364469296367737616, ; 483: System.Reflection.Emit.ILGeneration.dll => 0x8fd5fde967711b10 => 90
	i64 10376576884623852283, ; 484: Xamarin.AndroidX.Tracing.Tracing => 0x900101b2f888c2fb => 330
	i64 10389735884413426201, ; 485: fr\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x902fc1bd5c975a19 => 381
	i64 10406448008575299332, ; 486: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 350
	i64 10426284384445314437, ; 487: es\Microsoft.CodeAnalysis.resources => 0x90b19a682610b585 => 354
	i64 10430153318873392755, ; 488: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 289
	i64 10447083246144586668, ; 489: Microsoft.Bcl.AsyncInterfaces.dll => 0x90fb7edc816203ac => 182
	i64 10448061532586656644, ; 490: System.Composition.Convention.dll => 0x90fef89b91404384 => 256
	i64 10503238815856555353, ; 491: ko\Microsoft.CodeAnalysis.resources => 0x91c3000df2397559 => 358
	i64 10506226065143327199, ; 492: ca\Microsoft.Maui.Controls.resources => 0x91cd9cf11ed169df => 405
	i64 10546663366131771576, ; 493: System.Runtime.Serialization.Json.dll => 0x925d4673efe8e8b8 => 112
	i64 10566960649245365243, ; 494: System.Globalization.dll => 0x92a562b96dcd13fb => 42
	i64 10568800835145017495, ; 495: tr/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0x92abec5d18aba897 => 401
	i64 10595762989148858956, ; 496: System.Xml.XPath.XDocument => 0x930bb64cc472ea4c => 159
	i64 10608048793998982137, ; 497: Microsoft.Kiota.Serialization.Json.dll => 0x93375c2c9e51fbf9 => 223
	i64 10611392064737558276, ; 498: ko/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x93433cdc7f14bf04 => 384
	i64 10618541785153769341, ; 499: Syncfusion.Maui.Calendar => 0x935ca37e80bb8f7d => 251
	i64 10642885958238400069, ; 500: Syncfusion.Maui.Calendar.dll => 0x93b32063fdcaee45 => 251
	i64 10670374202010151210, ; 501: Microsoft.Win32.Primitives.dll => 0x9414c8cd7b4ea92a => 4
	i64 10714184849103829812, ; 502: System.Runtime.Extensions.dll => 0x94b06e5aa4b4bb34 => 103
	i64 10764062273958828890, ; 503: it\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0x9561a19b937fdb5a => 382
	i64 10785150219063592792, ; 504: System.Net.Primitives => 0x95ac8cfb68830758 => 70
	i64 10811915265162633087, ; 505: Microsoft.EntityFrameworkCore.Relational.dll => 0x960ba3a651a45f7f => 195
	i64 10821903387889976827, ; 506: fr/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0x962f1fcb5cc745fb => 381
	i64 10822644899632537592, ; 507: System.Linq.Queryable => 0x9631c23204ca5ff8 => 60
	i64 10830817578243619689, ; 508: System.Formats.Tar => 0x964ecb340a447b69 => 39
	i64 10847732767863316357, ; 509: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 279
	i64 10881075916135721761, ; 510: Microsoft.Graph.Core => 0x970158e53353eb21 => 211
	i64 10885087467875303060, ; 511: K4os.Compression.LZ4.Streams => 0x970f99615fc37e94 => 180
	i64 10899834349646441345, ; 512: System.Web => 0x9743fd975946eb81 => 153
	i64 10943875058216066601, ; 513: System.IO.UnmanagedMemoryStream.dll => 0x97e07461df39de29 => 56
	i64 10964653383833615866, ; 514: System.Diagnostics.Tracing => 0x982a4628ccaffdfa => 34
	i64 11002576679268595294, ; 515: Microsoft.Extensions.Logging.Abstractions => 0x98b1013215cd365e => 206
	i64 11009005086950030778, ; 516: Microsoft.Maui.dll => 0x98c7d7cc621ffdba => 229
	i64 11009991250214055283, ; 517: Serilog.Settings.Configuration => 0x98cb58b5692aed73 => 239
	i64 11011906640654766267, ; 518: pl/Microsoft.CodeAnalysis.CSharp.resources.dll => 0x98d226befffe10bb => 372
	i64 11019817191295005410, ; 519: Xamarin.AndroidX.Annotation.Jvm.dll => 0x98ee415998e1b2e2 => 276
	i64 11023048688141570732, ; 520: System.Core => 0x98f9bc61168392ac => 21
	i64 11037814507248023548, ; 521: System.Xml => 0x992e31d0412bf7fc => 163
	i64 11071824625609515081, ; 522: Xamarin.Google.ErrorProne.Annotations => 0x99a705d600e0a049 => 342
	i64 11103970607964515343, ; 523: hu\Microsoft.Maui.Controls.resources => 0x9a193a6fc41a6c0f => 416
	i64 11136029745144976707, ; 524: Jsr305Binding.dll => 0x9a8b200d4f8cd543 => 340
	i64 11162124722117608902, ; 525: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 335
	i64 11188319605227840848, ; 526: System.Threading.Overlapped => 0x9b44e5671724e550 => 140
	i64 11220793807500858938, ; 527: ja\Microsoft.Maui.Controls.resources => 0x9bb8448481fdd63a => 419
	i64 11226290749488709958, ; 528: Microsoft.Extensions.Options.dll => 0x9bcbcbf50c874146 => 208
	i64 11235648312900863002, ; 529: System.Reflection.DispatchProxy.dll => 0x9bed0a9c8fac441a => 89
	i64 11329751333533450475, ; 530: System.Threading.Timer.dll => 0x9d3b5ccf6cc500eb => 147
	i64 11340910727871153756, ; 531: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 291
	i64 11341245327015630248, ; 532: System.Configuration.ConfigurationManager.dll => 0x9d643289535355a8 => 260
	i64 11347436699239206956, ; 533: System.Xml.XmlSerializer.dll => 0x9d7a318e8162502c => 162
	i64 11392833485892708388, ; 534: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 320
	i64 11398376662953476300, ; 535: Microsoft.EntityFrameworkCore.Sqlite.dll => 0x9e2f2b2f0b71c0cc => 196
	i64 11432101114902388181, ; 536: System.AppContext => 0x9ea6fb64e61a9dd5 => 6
	i64 11446671985764974897, ; 537: Mono.Android.Export => 0x9edabf8623efc131 => 169
	i64 11448276831755070604, ; 538: System.Diagnostics.TextWriterTraceListener => 0x9ee0731f77186c8c => 31
	i64 11485890710487134646, ; 539: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 107
	i64 11508496261504176197, ; 540: Xamarin.AndroidX.Fragment.Ktx.dll => 0x9fb664600dde1045 => 301
	i64 11513602507638267977, ; 541: System.IO.Pipelines.dll => 0x9fc8887aa0d36049 => 263
	i64 11517440453979132662, ; 542: Microsoft.IdentityModel.Abstractions.dll => 0x9fd62b122523d2f6 => 212
	i64 11518296021396496455, ; 543: id\Microsoft.Maui.Controls.resources => 0x9fd9353475222047 => 417
	i64 11529969570048099689, ; 544: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 335
	i64 11530571088791430846, ; 545: Microsoft.Extensions.Logging => 0xa004d1504ccd66be => 205
	i64 11531031314636890415, ; 546: Microsoft.Kiota.Abstractions => 0xa00673e2fad6612f => 219
	i64 11537921547193889552, ; 547: Roslyn.VisualStudio.Services.UnitTests.dll => 0xa01eee8442d13310 => 235
	i64 11564861549255168062, ; 548: Microsoft.CodeAnalysis.dll => 0xa07ea44e47ed903e => 183
	i64 11580057168383206117, ; 549: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 274
	i64 11591352189662810718, ; 550: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0xa0dcc167234c525e => 328
	i64 11597940890313164233, ; 551: netstandard => 0xa0f429ca8d1805c9 => 167
	i64 11672361001936329215, ; 552: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 302
	i64 11690125841757694131, ; 553: es/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xa23bab842194e8b3 => 393
	i64 11691353810037938030, ; 554: pl\Microsoft.CodeAnalysis.resources => 0xa2400858c6b8976e => 359
	i64 11692977985522001935, ; 555: System.Threading.Overlapped.dll => 0xa245cd869980680f => 140
	i64 11705530742807338875, ; 556: he/Microsoft.Maui.Controls.resources.dll => 0xa272663128721f7b => 413
	i64 11707554492040141440, ; 557: System.Linq.Parallel.dll => 0xa27996c7fe94da80 => 59
	i64 11739066727115742305, ; 558: SQLite-net.dll => 0xa2e98afdf8575c61 => 242
	i64 11743665907891708234, ; 559: System.Threading.Tasks => 0xa2f9e1ec30c0214a => 144
	i64 11806260347154423189, ; 560: SQLite-net => 0xa3d8433bc5eb5d95 => 242
	i64 11851835839753101194, ; 561: Microsoft.CodeAnalysis.CSharp.Workspaces.dll => 0xa47a2de70cba1f8a => 186
	i64 11902187774554328846, ; 562: Roslynator.Interfaces.dll => 0xa52d10b8704ecf0e => 236
	i64 11991047634523762324, ; 563: System.Net => 0xa668c24ad493ae94 => 81
	i64 11997281068246925203, ; 564: Microsoft.CodeAnalysis.VisualBasic.Workspaces => 0xa67ee79137607f93 => 190
	i64 12011556116648931059, ; 565: System.Security.Cryptography.ProtectedData => 0xa6b19ea5ec87aef3 => 265
	i64 12040886584167504988, ; 566: System.Net.ServicePoint => 0xa719d28d8e121c5c => 74
	i64 12063623837170009990, ; 567: System.Security => 0xa76a99f6ce740786 => 130
	i64 12096697103934194533, ; 568: System.Diagnostics.Contracts => 0xa7e019eccb7e8365 => 25
	i64 12102847907131387746, ; 569: System.Buffers => 0xa7f5f40c43256f62 => 7
	i64 12123043025855404482, ; 570: System.Reflection.Extensions.dll => 0xa83db366c0e359c2 => 93
	i64 12137774235383566651, ; 571: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 332
	i64 12145679461940342714, ; 572: System.Text.Json => 0xa88e1f1ebcb62fba => 137
	i64 12191646537372739477, ; 573: Xamarin.Android.Glide.dll => 0xa9316dee7f392795 => 268
	i64 12191826543680975904, ; 574: Microsoft.Kiota.Abstractions.dll => 0xa93211a57b487420 => 219
	i64 12198439281774268251, ; 575: Microsoft.IdentityModel.Protocols.OpenIdConnect.dll => 0xa9498fe58c538f5b => 216
	i64 12201331334810686224, ; 576: System.Runtime.Serialization.Primitives.dll => 0xa953d6341e3bd310 => 113
	i64 12269460666702402136, ; 577: System.Collections.Immutable.dll => 0xaa45e178506c9258 => 9
	i64 12279246230491828964, ; 578: SQLitePCLRaw.provider.e_sqlite3.dll => 0xaa68a5636e0512e4 => 248
	i64 12286384399996964502, ; 579: Microsoft.Graph.Core.dll => 0xaa82018407b84e96 => 211
	i64 12313367145828839434, ; 580: System.IO.Pipelines => 0xaae1de2e1c17f00a => 263
	i64 12332222936682028543, ; 581: System.Runtime.Handles => 0xab24db6c07db5dff => 104
	i64 12375446203996702057, ; 582: System.Configuration.dll => 0xabbe6ac12e2e0569 => 19
	i64 12439275739440478309, ; 583: Microsoft.IdentityModel.JsonWebTokens => 0xaca12f61007bf865 => 213
	i64 12451044538927396471, ; 584: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 300
	i64 12466513435562512481, ; 585: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 314
	i64 12475113361194491050, ; 586: _Microsoft.Android.Resource.Designer.dll => 0xad2081818aba1caa => 438
	i64 12487638416075308985, ; 587: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 294
	i64 12517810545449516888, ; 588: System.Diagnostics.TraceSource.dll => 0xadb8325e6f283f58 => 33
	i64 12538491095302438457, ; 589: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 282
	i64 12550732019250633519, ; 590: System.IO.Compression => 0xae2d28465e8e1b2f => 46
	i64 12551451704392164662, ; 591: MySqlConnector.dll => 0xae2fb6d31fc42536 => 233
	i64 12597257347146224972, ; 592: zh-Hans/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xaed272d068d4954c => 389
	i64 12619191878741427339, ; 593: de\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xaf20602784e0848b => 379
	i64 12681088699309157496, ; 594: it/Microsoft.Maui.Controls.resources.dll => 0xaffc46fc178aec78 => 418
	i64 12699999919562409296, ; 595: System.Diagnostics.StackTrace.dll => 0xb03f76a3ad01c550 => 30
	i64 12700543734426720211, ; 596: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 283
	i64 12701943080736688682, ; 597: Microsoft.Kiota.Authentication.Azure => 0xb0465def248a8a2a => 220
	i64 12708238894395270091, ; 598: System.IO => 0xb05cbbf17d3ba3cb => 57
	i64 12708922737231849740, ; 599: System.Text.Encoding.Extensions => 0xb05f29e50e96e90c => 134
	i64 12717050818822477433, ; 600: System.Runtime.Serialization.Xml.dll => 0xb07c0a5786811679 => 114
	i64 12753841065332862057, ; 601: Xamarin.AndroidX.Window => 0xb0febee04cf46c69 => 337
	i64 12822330414412999099, ; 602: zh-Hant\Microsoft.CodeAnalysis.CSharp.resources => 0xb1f2119387c629bb => 377
	i64 12823819093633476069, ; 603: th/Microsoft.Maui.Controls.resources.dll => 0xb1f75b85abe525e5 => 431
	i64 12828192437253469131, ; 604: Xamarin.Kotlin.StdLib.Jdk8.dll => 0xb206e50e14d873cb => 348
	i64 12831179148886114003, ; 605: it/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xb211817412c17ed3 => 369
	i64 12835242264250840079, ; 606: System.IO.Pipes => 0xb21ff0d5d6c0740f => 55
	i64 12835543923467107475, ; 607: pt-BR\Microsoft.CodeAnalysis.resources => 0xb2210331592e3c93 => 360
	i64 12843321153144804894, ; 608: Microsoft.Extensions.Primitives => 0xb23ca48abd74d61e => 209
	i64 12843770487262409629, ; 609: System.AppContext.dll => 0xb23e3d357debf39d => 6
	i64 12859557719246324186, ; 610: System.Net.WebHeaderCollection.dll => 0xb276539ce04f41da => 77
	i64 12982280885948128408, ; 611: Xamarin.AndroidX.CustomView.PoolingContainer => 0xb42a53aec5481c98 => 293
	i64 12991459499837607210, ; 612: Microsoft.CodeAnalysis => 0xb44aef9559b1cd2a => 183
	i64 13039467033719597668, ; 613: tr/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xb4f57e2e5d45ea64 => 375
	i64 13068258254871114833, ; 614: System.Runtime.Serialization.Formatters.dll => 0xb55bc7a4eaa8b451 => 111
	i64 13126023683090012938, ; 615: System.Composition.TypedParts.dll => 0xb62900febff1db0a => 259
	i64 13129914918964716986, ; 616: Xamarin.AndroidX.Emoji2.dll => 0xb636d40db3fe65ba => 297
	i64 13161648197185303412, ; 617: ru\Microsoft.CodeAnalysis.Workspaces.resources => 0xb6a7914d4be3f374 => 400
	i64 13162471042547327635, ; 618: System.Security.Permissions => 0xb6aa7dace9662293 => 266
	i64 13166897321255124987, ; 619: ko\Microsoft.CodeAnalysis.CSharp.resources => 0xb6ba375a3b743ffb => 371
	i64 13173818576982874404, ; 620: System.Runtime.CompilerServices.VisualC.dll => 0xb6d2ce32a8819924 => 102
	i64 13221551921002590604, ; 621: ca/Microsoft.Maui.Controls.resources.dll => 0xb77c636bdebe318c => 405
	i64 13222659110913276082, ; 622: ja/Microsoft.Maui.Controls.resources.dll => 0xb78052679c1178b2 => 419
	i64 13262938754463820554, ; 623: zh-Hans\Microsoft.CodeAnalysis.Workspaces.resources => 0xb80f6c86f193eb0a => 402
	i64 13270034446771288861, ; 624: zh-Hant/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xb828a2058cfffb1d => 377
	i64 13343850469010654401, ; 625: Mono.Android.Runtime.dll => 0xb92ee14d854f44c1 => 170
	i64 13370592475155966277, ; 626: System.Runtime.Serialization => 0xb98de304062ea945 => 115
	i64 13381594904270902445, ; 627: he\Microsoft.Maui.Controls.resources => 0xb9b4f9aaad3e94ad => 413
	i64 13401370062847626945, ; 628: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 332
	i64 13404347523447273790, ; 629: Xamarin.AndroidX.ConstraintLayout.Core => 0xba05cf0da4f6393e => 287
	i64 13431476299110033919, ; 630: System.Net.WebClient => 0xba663087f18829ff => 76
	i64 13454009404024712428, ; 631: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 343
	i64 13463706743370286408, ; 632: System.Private.DataContractSerialization.dll => 0xbad8b1f3069e0548 => 85
	i64 13465488254036897740, ; 633: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 345
	i64 13467053111158216594, ; 634: uk/Microsoft.Maui.Controls.resources.dll => 0xbae49573fde79792 => 433
	i64 13491513212026656886, ; 635: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 280
	i64 13502641473732064860, ; 636: System.Composition.AttributedModel => 0xbb6304e15b41b65c => 255
	i64 13540124433173649601, ; 637: vi\Microsoft.Maui.Controls.resources => 0xbbe82f6eede718c1 => 434
	i64 13545416393490209236, ; 638: id/Microsoft.Maui.Controls.resources.dll => 0xbbfafc7174bc99d4 => 417
	i64 13572454107664307259, ; 639: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 322
	i64 13578472628727169633, ; 640: System.Xml.XPath => 0xbc706ce9fba5c261 => 160
	i64 13580399111273692417, ; 641: Microsoft.VisualBasic.Core.dll => 0xbc77450a277fbd01 => 2
	i64 13621154251410165619, ; 642: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0xbd080f9faa1acf73 => 293
	i64 13647894001087880694, ; 643: System.Data.dll => 0xbd670f48cb071df6 => 24
	i64 13675589307506966157, ; 644: Xamarin.AndroidX.Activity.Ktx => 0xbdc97404d0153e8d => 273
	i64 13702626353344114072, ; 645: System.Diagnostics.Tools.dll => 0xbe29821198fb6d98 => 32
	i64 13710614125866346983, ; 646: System.Security.AccessControl.dll => 0xbe45e2e7d0b769e7 => 117
	i64 13713329104121190199, ; 647: System.Dynamic.Runtime => 0xbe4f8829f32b5737 => 37
	i64 13717397318615465333, ; 648: System.ComponentModel.Primitives.dll => 0xbe5dfc2ef2f87d75 => 16
	i64 13755568601956062840, ; 649: fr/Microsoft.Maui.Controls.resources.dll => 0xbee598c36b1b9678 => 412
	i64 13768883594457632599, ; 650: System.IO.IsolatedStorage => 0xbf14e6adb159cf57 => 52
	i64 13803157159895053221, ; 651: ru/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xbf8eaa4dfe6f33a5 => 387
	i64 13814445057219246765, ; 652: hr/Microsoft.Maui.Controls.resources.dll => 0xbfb6c49664b43aad => 415
	i64 13828521679616088467, ; 653: Xamarin.Kotlin.StdLib.Common => 0xbfe8c733724e1993 => 346
	i64 13864745339496195545, ; 654: Microsoft.CodeAnalysis.Workspaces.dll => 0xc069786d7dec65d9 => 191
	i64 13881769479078963060, ; 655: System.Console.dll => 0xc0a5f3cade5c6774 => 20
	i64 13882652712560114096, ; 656: System.Windows.Extensions.dll => 0xc0a91716b04239b0 => 267
	i64 13911222732217019342, ; 657: System.Security.Cryptography.OpenSsl.dll => 0xc10e975ec1226bce => 123
	i64 13928444506500929300, ; 658: System.Windows.dll => 0xc14bc67b8bba9714 => 154
	i64 13955418299340266673, ; 659: Microsoft.Extensions.DependencyModel.dll => 0xc1ab9b0118299cb1 => 204
	i64 13959074834287824816, ; 660: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 300
	i64 13970307180132182141, ; 661: Syncfusion.Licensing => 0xc1e0805ccade287d => 250
	i64 14075334701871371868, ; 662: System.ServiceModel.Web.dll => 0xc355a25647c5965c => 131
	i64 14100563506285742564, ; 663: da/Microsoft.Maui.Controls.resources.dll => 0xc3af43cd0cff89e4 => 407
	i64 14124974489674258913, ; 664: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 282
	i64 14125464355221830302, ; 665: System.Threading.dll => 0xc407bafdbc707a9e => 148
	i64 14133832980772275001, ; 666: Microsoft.EntityFrameworkCore.dll => 0xc425763635a1c339 => 193
	i64 14178052285788134900, ; 667: Xamarin.Android.Glide.Annotations.dll => 0xc4c28f6f75511df4 => 269
	i64 14212104595480609394, ; 668: System.Security.Cryptography.Cng.dll => 0xc53b89d4a4518272 => 120
	i64 14220608275227875801, ; 669: System.Diagnostics.FileVersionInfo.dll => 0xc559bfe1def019d9 => 28
	i64 14226382999226559092, ; 670: System.ServiceProcess => 0xc56e43f6938e2a74 => 132
	i64 14232023429000439693, ; 671: System.Resources.Writer.dll => 0xc5824de7789ba78d => 100
	i64 14236779789349124699, ; 672: cs/Microsoft.CodeAnalysis.resources.dll => 0xc59333c9e99d7a5b => 352
	i64 14254574811015963973, ; 673: System.Text.Encoding.Extensions.dll => 0xc5d26c4442d66545 => 134
	i64 14258050503687732042, ; 674: zh-Hant\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xc5dec56405e2fb4a => 390
	i64 14261073672896646636, ; 675: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 320
	i64 14298246716367104064, ; 676: System.Web.dll => 0xc66d93a217f4e840 => 153
	i64 14327695147300244862, ; 677: System.Reflection.dll => 0xc6d632d338eb4d7e => 97
	i64 14327709162229390963, ; 678: System.Security.Cryptography.X509Certificates => 0xc6d63f9253cade73 => 125
	i64 14331727281556788554, ; 679: Xamarin.Android.Glide.DiskLruCache.dll => 0xc6e48607a2f7954a => 270
	i64 14346402571976470310, ; 680: System.Net.Ping.dll => 0xc718a920f3686f26 => 69
	i64 14445633579937531325, ; 681: Microsoft.Graph => 0xc879333467a185bd => 210
	i64 14461014870687870182, ; 682: System.Net.Requests.dll => 0xc8afd8683afdece6 => 72
	i64 14464374589798375073, ; 683: ru\Microsoft.Maui.Controls.resources => 0xc8bbc80dcb1e5ea1 => 428
	i64 14486659737292545672, ; 684: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 305
	i64 14495724990987328804, ; 685: Xamarin.AndroidX.ResourceInspection.Annotation => 0xc92b2913e18d5d24 => 323
	i64 14522721392235705434, ; 686: el/Microsoft.Maui.Controls.resources.dll => 0xc98b12295c2cf45a => 409
	i64 14524899859764431419, ; 687: Bingie.dll => 0xc992cf775b614a3b => 0
	i64 14538127318538747197, ; 688: Syncfusion.Licensing.dll => 0xc9c1cdc518e77d3d => 250
	i64 14551742072151931844, ; 689: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 136
	i64 14561513370130550166, ; 690: System.Security.Cryptography.Primitives.dll => 0xca14e3428abb8d96 => 124
	i64 14574160591280636898, ; 691: System.Net.Quic => 0xca41d1d72ec783e2 => 71
	i64 14622043554576106986, ; 692: System.Runtime.Serialization.Formatters => 0xcaebef2458cc85ea => 111
	i64 14644440854989303794, ; 693: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 315
	i64 14669215534098758659, ; 694: Microsoft.Extensions.DependencyInjection.dll => 0xcb9385ceb3993c03 => 202
	i64 14681384888972678063, ; 695: Microsoft.Kiota.Serialization.Form => 0xcbbec1c56e0113af => 222
	i64 14690985099581930927, ; 696: System.Web.HttpUtility => 0xcbe0dd1ca5233daf => 152
	i64 14705122255218365489, ; 697: ko\Microsoft.Maui.Controls.resources => 0xcc1316c7b0fb5431 => 420
	i64 14744092281598614090, ; 698: zh-Hans\Microsoft.Maui.Controls.resources => 0xcc9d89d004439a4a => 436
	i64 14792063746108907174, ; 699: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 343
	i64 14822143737991655809, ; 700: Serilog.Extensions.Logging => 0xcdb2d532d8c5f181 => 238
	i64 14832630590065248058, ; 701: System.Security.Claims => 0xcdd816ef5d6e873a => 118
	i64 14852515768018889994, ; 702: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 291
	i64 14889905118082851278, ; 703: GoogleGson.dll => 0xcea391d0969961ce => 177
	i64 14892012299694389861, ; 704: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xceab0e490a083a65 => 437
	i64 14904040806490515477, ; 705: ar\Microsoft.Maui.Controls.resources => 0xced5ca2604cb2815 => 404
	i64 14912225920358050525, ; 706: System.Security.Principal.Windows => 0xcef2de7759506add => 127
	i64 14935719434541007538, ; 707: System.Text.Encoding.CodePages.dll => 0xcf4655b160b702b2 => 133
	i64 14954917835170835695, ; 708: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xcf8a8a895a82ecef => 203
	i64 14984936317414011727, ; 709: System.Net.WebHeaderCollection => 0xcff5302fe54ff34f => 77
	i64 14987728460634540364, ; 710: System.IO.Compression.dll => 0xcfff1ba06622494c => 46
	i64 14988210264188246988, ; 711: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 294
	i64 15015154896917945444, ; 712: System.Net.Security.dll => 0xd0608bd33642dc64 => 73
	i64 15024878362326791334, ; 713: System.Net.Http.Json => 0xd0831743ebf0f4a6 => 63
	i64 15071021337266399595, ; 714: System.Resources.Reader.dll => 0xd127060e7a18a96b => 98
	i64 15076659072870671916, ; 715: System.ObjectModel.dll => 0xd13b0d8c1620662c => 84
	i64 15111608613780139878, ; 716: ms\Microsoft.Maui.Controls.resources => 0xd1b737f831192f66 => 421
	i64 15115185479366240210, ; 717: System.IO.Compression.Brotli.dll => 0xd1c3ed1c1bc467d2 => 43
	i64 15133485256822086103, ; 718: System.Linq.dll => 0xd204f0a9127dd9d7 => 61
	i64 15138356091203993725, ; 719: Microsoft.IdentityModel.Abstractions => 0xd2163ea89395c07d => 212
	i64 15150743910298169673, ; 720: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xd2424150783c3149 => 321
	i64 15195733091524337868, ; 721: ja\Microsoft.CodeAnalysis.resources => 0xd2e216bc7df4e0cc => 357
	i64 15227001540531775957, ; 722: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd3512d3999b8e9d5 => 200
	i64 15234786388537674379, ; 723: System.Dynamic.Runtime.dll => 0xd36cd580c5be8a8b => 37
	i64 15250465174479574862, ; 724: System.Globalization.Calendars.dll => 0xd3a489469852174e => 40
	i64 15272359115529052076, ; 725: Xamarin.AndroidX.Collection.Ktx => 0xd3f251b2fb4edfac => 284
	i64 15279429628684179188, ; 726: Xamarin.KotlinX.Coroutines.Android.dll => 0xd40b704b1c4c96f4 => 349
	i64 15299439993936780255, ; 727: System.Xml.XPath.dll => 0xd452879d55019bdf => 160
	i64 15300862763834473199, ; 728: System.Composition.Hosting.dll => 0xd457959dc35afaef => 257
	i64 15338463749992804988, ; 729: System.Resources.Reader => 0xd4dd2b839286f27c => 98
	i64 15344154949114261798, ; 730: fr/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xd4f163a12081f126 => 368
	i64 15352427450275134006, ; 731: System.Composition.Runtime.dll => 0xd50ec76ce59afa36 => 258
	i64 15370334346939861994, ; 732: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 289
	i64 15376059877735500810, ; 733: es/Microsoft.CodeAnalysis.resources.dll => 0xd562bcfe318f8c0a => 354
	i64 15383240894167415497, ; 734: System.Memory.Data => 0xd57c4016df1c7ac9 => 264
	i64 15389372189903242610, ; 735: zh-Hans/Microsoft.CodeAnalysis.resources.dll => 0xd592087867754572 => 363
	i64 15391712275433856905, ; 736: Microsoft.Extensions.DependencyInjection.Abstractions => 0xd59a58c406411f89 => 203
	i64 15450811188137282119, ; 737: ja/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xd66c4ee9e88dca47 => 396
	i64 15475196252089753159, ; 738: System.Diagnostics.EventLog => 0xd6c2f1000b441e47 => 261
	i64 15508190622468184067, ; 739: Bingie => 0xd738293489429803 => 0
	i64 15526743539506359484, ; 740: System.Text.Encoding.dll => 0xd77a12fc26de2cbc => 135
	i64 15527772828719725935, ; 741: System.Console => 0xd77dbb1e38cd3d6f => 20
	i64 15530465045505749832, ; 742: System.Net.HttpListener.dll => 0xd7874bacc9fdb348 => 65
	i64 15536481058354060254, ; 743: de\Microsoft.Maui.Controls.resources => 0xd79cab34eec75bde => 408
	i64 15541854775306130054, ; 744: System.Security.Cryptography.X509Certificates.dll => 0xd7afc292e8d49286 => 125
	i64 15557562860424774966, ; 745: System.Net.Sockets => 0xd7e790fe7a6dc536 => 75
	i64 15582737692548360875, ; 746: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 313
	i64 15609085926864131306, ; 747: System.dll => 0xd89e9cf3334914ea => 164
	i64 15620595871140898079, ; 748: Microsoft.Extensions.DependencyModel => 0xd8c7812eef49651f => 204
	i64 15620612276725577442, ; 749: BouncyCastle.Cryptography.dll => 0xd8c7901aa85576e2 => 175
	i64 15661133872274321916, ; 750: System.Xml.ReaderWriter.dll => 0xd9578647d4bfb1fc => 156
	i64 15664356999916475676, ; 751: de/Microsoft.Maui.Controls.resources.dll => 0xd962f9b2b6ecd51c => 408
	i64 15668223562483655067, ; 752: Roslyn.VisualStudio.Services.UnitTests => 0xd970b650f73e619b => 235
	i64 15710114879900314733, ; 753: Microsoft.Win32.Registry => 0xda058a3f5d096c6d => 5
	i64 15743187114543869802, ; 754: hu/Microsoft.Maui.Controls.resources.dll => 0xda7b09450ae4ef6a => 416
	i64 15745825835632158716, ; 755: Syncfusion.Maui.Core => 0xda84692c2c05e7fc => 252
	i64 15755368083429170162, ; 756: System.IO.FileSystem.Primitives => 0xdaa64fcbde529bf2 => 49
	i64 15777549416145007739, ; 757: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 327
	i64 15783653065526199428, ; 758: el\Microsoft.Maui.Controls.resources => 0xdb0accd674b1c484 => 409
	i64 15817206913877585035, ; 759: System.Threading.Tasks.dll => 0xdb8201e29086ac8b => 144
	i64 15847085070278954535, ; 760: System.Threading.Channels.dll => 0xdbec27e8f35f8e27 => 139
	i64 15878195890548581965, ; 761: zh-Hans\Microsoft.CodeAnalysis.CSharp.Workspaces.resources => 0xdc5aaf094237264d => 389
	i64 15885744048853936810, ; 762: System.Resources.Writer => 0xdc75800bd0b6eaaa => 100
	i64 15928521404965645318, ; 763: Microsoft.Maui.Controls.Compatibility => 0xdd0d79d32c2eec06 => 226
	i64 15934062614519587357, ; 764: System.Security.Cryptography.OpenSsl => 0xdd2129868f45a21d => 123
	i64 15937190497610202713, ; 765: System.Security.Cryptography.Cng => 0xdd2c465197c97e59 => 120
	i64 15963349826457351533, ; 766: System.Threading.Tasks.Extensions => 0xdd893616f748b56d => 142
	i64 15971679995444160383, ; 767: System.Formats.Tar.dll => 0xdda6ce5592a9677f => 39
	i64 16018552496348375205, ; 768: System.Net.NetworkInformation.dll => 0xde4d54a020caa8a5 => 68
	i64 16053592439138609341, ; 769: pt-BR/Microsoft.CodeAnalysis.resources.dll => 0xdec9d1448fc0e8bd => 360
	i64 16054465462676478687, ; 770: System.Globalization.Extensions => 0xdecceb47319bdadf => 41
	i64 16154507427712707110, ; 771: System => 0xe03056ea4e39aa26 => 164
	i64 16219561732052121626, ; 772: System.Net.Security => 0xe1177575db7c781a => 73
	i64 16288847719894691167, ; 773: nb\Microsoft.Maui.Controls.resources => 0xe20d9cb300c12d5f => 422
	i64 16315482530584035869, ; 774: WindowsBase.dll => 0xe26c3ceb1e8d821d => 165
	i64 16321164108206115771, ; 775: Microsoft.Extensions.Logging.Abstractions.dll => 0xe2806c487e7b0bbb => 206
	i64 16337011941688632206, ; 776: System.Security.Principal.Windows.dll => 0xe2b8b9cdc3aa638e => 127
	i64 16361933716545543812, ; 777: Xamarin.AndroidX.ExifInterface.dll => 0xe3114406a52f1e84 => 299
	i64 16423015068819898779, ; 778: Xamarin.Kotlin.StdLib.Jdk8 => 0xe3ea453135e5c19b => 348
	i64 16454459195343277943, ; 779: System.Net.NetworkInformation => 0xe459fb756d988f77 => 68
	i64 16496768397145114574, ; 780: Mono.Android.Export.dll => 0xe4f04b741db987ce => 169
	i64 16572092361255939259, ; 781: Microsoft.IdentityModel.Validators.dll => 0xe5fbe633299fecbb => 218
	i64 16589693266713801121, ; 782: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xe63a6e214f2a71a1 => 312
	i64 16621146507174665210, ; 783: Xamarin.AndroidX.ConstraintLayout => 0xe6aa2caf87dedbfa => 286
	i64 16637862623548895820, ; 784: K4os.Compression.LZ4 => 0xe6e58fe7aa61724c => 179
	i64 16643194905613199096, ; 785: System.Composition.Runtime => 0xe6f8819654aa66f8 => 258
	i64 16649148416072044166, ; 786: Microsoft.Maui.Graphics => 0xe70da84600bb4e86 => 231
	i64 16669448769200862260, ; 787: Serilog.Sinks.File => 0xe755c75649ca3434 => 241
	i64 16677317093839702854, ; 788: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 319
	i64 16702652415771857902, ; 789: System.ValueTuple => 0xe7cbbde0b0e6d3ee => 151
	i64 16709499819875633724, ; 790: System.IO.Compression.ZipFile => 0xe7e4118e32240a3c => 45
	i64 16737807731308835127, ; 791: System.Runtime.Intrinsics => 0xe848a3736f733137 => 108
	i64 16755018182064898362, ; 792: SQLitePCLRaw.core.dll => 0xe885c843c330813a => 244
	i64 16758309481308491337, ; 793: System.IO.FileSystem.DriveInfo => 0xe89179af15740e49 => 48
	i64 16762783179241323229, ; 794: System.Reflection.TypeExtensions => 0xe8a15e7d0d927add => 96
	i64 16765015072123548030, ; 795: System.Diagnostics.TextWriterTraceListener.dll => 0xe8a94c621bfe717e => 31
	i64 16822611501064131242, ; 796: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 23
	i64 16833383113903931215, ; 797: mscorlib => 0xe99c30c1484d7f4f => 166
	i64 16856067890322379635, ; 798: System.Data.Common.dll => 0xe9ecc87060889373 => 22
	i64 16873478996345296124, ; 799: K4os.Compression.LZ4.Streams.dll => 0xea2aa3bf662d14fc => 180
	i64 16890310621557459193, ; 800: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 138
	i64 16933958494752847024, ; 801: System.Net.WebProxy.dll => 0xeb018187f0f3b4b0 => 78
	i64 16942731696432749159, ; 802: sk\Microsoft.Maui.Controls.resources => 0xeb20acb622a01a67 => 429
	i64 16945858842201062480, ; 803: Azure.Core => 0xeb2bc8d57f4e7c50 => 173
	i64 16977952268158210142, ; 804: System.IO.Pipes.AccessControl => 0xeb9dcda2851b905e => 54
	i64 16989020923549080504, ; 805: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xebc52084add25bb8 => 312
	i64 16998075588627545693, ; 806: Xamarin.AndroidX.Navigation.Fragment => 0xebe54bb02d623e5d => 317
	i64 17000529083408271384, ; 807: Std.UriTemplate => 0xebee0320f23f4818 => 249
	i64 17006954551347072385, ; 808: System.ClientModel => 0xec04d70ec8414d81 => 254
	i64 17008137082415910100, ; 809: System.Collections.NonGeneric => 0xec090a90408c8cd4 => 10
	i64 17024911836938395553, ; 810: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 275
	i64 17031295164305207763, ; 811: Microsoft.CodeAnalysis.Features => 0xec5b50b75d3d71d3 => 187
	i64 17031351772568316411, ; 812: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 316
	i64 17033754095083596056, ; 813: Microsoft.Kiota.Serialization.Multipart.dll => 0xec640d19ccd02918 => 224
	i64 17037200463775726619, ; 814: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 303
	i64 17062143951396181894, ; 815: System.ComponentModel.Primitives => 0xecc8e986518c9786 => 16
	i64 17089008752050867324, ; 816: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xed285aeb25888c7c => 436
	i64 17093861341398734987, ; 817: it/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xed399852a5da988b => 382
	i64 17093953079634937686, ; 818: tr/Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll => 0xed39ebc21ed5c756 => 388
	i64 17118171214553292978, ; 819: System.Threading.Channels => 0xed8ff6060fc420b2 => 139
	i64 17137864900836977098, ; 820: Microsoft.IdentityModel.Tokens => 0xedd5ed53b705e9ca => 217
	i64 17187273293601214786, ; 821: System.ComponentModel.Annotations.dll => 0xee8575ff9aa89142 => 13
	i64 17201328579425343169, ; 822: System.ComponentModel.EventBasedAsync => 0xeeb76534d96c16c1 => 15
	i64 17202182880784296190, ; 823: System.Security.Cryptography.Encoding.dll => 0xeeba6e30627428fe => 122
	i64 17230721278011714856, ; 824: System.Private.Xml.Linq => 0xef1fd1b5c7a72d28 => 87
	i64 17234219099804750107, ; 825: System.Transactions.Local.dll => 0xef2c3ef5e11d511b => 149
	i64 17255271510338333894, ; 826: Roslynator.Interfaces => 0xef770a042ca26cc6 => 236
	i64 17260702271250283638, ; 827: System.Data.Common => 0xef8a5543bba6bc76 => 22
	i64 17319626546261031465, ; 828: Serilog.Sinks.Console.dll => 0xf05bac949c507a29 => 240
	i64 17333249706306540043, ; 829: System.Diagnostics.Tracing.dll => 0xf08c12c5bb8b920b => 34
	i64 17334349739241381640, ; 830: fr/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xf08ffb3f1bde1f08 => 394
	i64 17338386382517543202, ; 831: System.Net.WebSockets.Client.dll => 0xf09e528d5c6da122 => 79
	i64 17342750010158924305, ; 832: hi\Microsoft.Maui.Controls.resources => 0xf0add33f97ecc211 => 414
	i64 17360349973592121190, ; 833: Xamarin.Google.Crypto.Tink.Android => 0xf0ec5a52686b9f66 => 341
	i64 17438153253682247751, ; 834: sk/Microsoft.Maui.Controls.resources.dll => 0xf200c3fe308d7847 => 429
	i64 17451281075798943988, ; 835: Pomelo.EntityFrameworkCore.MySql => 0xf22f67ad767e28f4 => 234
	i64 17470386307322966175, ; 836: System.Threading.Timer => 0xf27347c8d0d5709f => 147
	i64 17504803799422154384, ; 837: Serilog.Sinks.File.dll => 0xf2ed8e4fa7773690 => 241
	i64 17509662556995089465, ; 838: System.Net.WebSockets.dll => 0xf2fed1534ea67439 => 80
	i64 17514990004910432069, ; 839: fr\Microsoft.Maui.Controls.resources => 0xf311be9c6f341f45 => 412
	i64 17522591619082469157, ; 840: GoogleGson => 0xf32cc03d27a5bf25 => 177
	i64 17523946658117960076, ; 841: System.Security.Cryptography.ProtectedData.dll => 0xf33190a3c403c18c => 265
	i64 17547642265084393538, ; 842: Microsoft.CodeAnalysis.VisualBasic => 0xf385bfab2ffc7842 => 188
	i64 17553799493972570483, ; 843: Google.Protobuf.dll => 0xf39b9fa2c0aab173 => 176
	i64 17590473451926037903, ; 844: Xamarin.Android.Glide => 0xf41dea67fcfda58f => 268
	i64 17613516908642624696, ; 845: cs/Microsoft.CodeAnalysis.Workspaces.resources.dll => 0xf46fc84ed8fae4b8 => 391
	i64 17623389608345532001, ; 846: pl\Microsoft.Maui.Controls.resources => 0xf492db79dfbef661 => 424
	i64 17627500474728259406, ; 847: System.Globalization => 0xf4a176498a351f4e => 42
	i64 17668681123506450143, ; 848: SQLitePCLRaw.provider.e_sqlcipher.dll => 0xf533c3de80406edf => 247
	i64 17685921127322830888, ; 849: System.Diagnostics.Debug.dll => 0xf571038fafa74828 => 26
	i64 17702523067201099846, ; 850: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xf5abfef008ae1846 => 435
	i64 17704177640604968747, ; 851: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 314
	i64 17710060891934109755, ; 852: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 311
	i64 17712670374920797664, ; 853: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 107
	i64 17777860260071588075, ; 854: System.Runtime.Numerics.dll => 0xf6b7a5b72419c0eb => 110
	i64 17790600151040787804, ; 855: Microsoft.IdentityModel.Logging => 0xf6e4e89427cc055c => 214
	i64 17838668724098252521, ; 856: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 7
	i64 17877291088293713194, ; 857: zh-Hans/Microsoft.CodeAnalysis.CSharp.resources.dll => 0xf818e586e000d52a => 376
	i64 17891337867145587222, ; 858: Xamarin.Jetbrains.Annotations => 0xf84accff6fb52a16 => 344
	i64 17928294245072900555, ; 859: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 44
	i64 17979120673405951447, ; 860: ZstdSharp => 0xf982aafeb83e5dd7 => 351
	i64 17992315986609351877, ; 861: System.Xml.XmlDocument.dll => 0xf9b18c0ffc6eacc5 => 161
	i64 18017743553296241350, ; 862: Microsoft.Extensions.Caching.Abstractions => 0xfa0be24cb44e92c6 => 197
	i64 18025913125965088385, ; 863: System.Threading => 0xfa28e87b91334681 => 148
	i64 18099568558057551825, ; 864: nl/Microsoft.Maui.Controls.resources.dll => 0xfb2e95b53ad977d1 => 423
	i64 18116111925905154859, ; 865: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 280
	i64 18121036031235206392, ; 866: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 316
	i64 18146411883821974900, ; 867: System.Formats.Asn1.dll => 0xfbd50176eb22c574 => 38
	i64 18146811631844267958, ; 868: System.ComponentModel.EventBasedAsync.dll => 0xfbd66d08820117b6 => 15
	i64 18225059387460068507, ; 869: System.Threading.ThreadPool.dll => 0xfcec6af3cff4a49b => 146
	i64 18245806341561545090, ; 870: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 8
	i64 18260797123374478311, ; 871: Xamarin.AndroidX.Emoji2 => 0xfd6b623bde35f3e7 => 297
	i64 18305135509493619199, ; 872: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 318
	i64 18318849532986632368, ; 873: System.Security.dll => 0xfe39a097c37fa8b0 => 130
	i64 18324163916253801303, ; 874: it\Microsoft.Maui.Controls.resources => 0xfe4c81ff0a56ab57 => 418
	i64 18370042311372477656, ; 875: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0xfeef80274e4094d8 => 246
	i64 18380184030268848184, ; 876: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 334
	i64 18439108438687598470 ; 877: System.Reflection.Metadata.dll => 0xffe4df6e2ee1c786 => 94
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [878 x i32] [
	i32 296, ; 0
	i32 234, ; 1
	i32 209, ; 2
	i32 171, ; 3
	i32 230, ; 4
	i32 178, ; 5
	i32 58, ; 6
	i32 283, ; 7
	i32 380, ; 8
	i32 151, ; 9
	i32 324, ; 10
	i32 327, ; 11
	i32 290, ; 12
	i32 132, ; 13
	i32 56, ; 14
	i32 326, ; 15
	i32 393, ; 16
	i32 351, ; 17
	i32 218, ; 18
	i32 411, ; 19
	i32 95, ; 20
	i32 237, ; 21
	i32 232, ; 22
	i32 184, ; 23
	i32 309, ; 24
	i32 129, ; 25
	i32 201, ; 26
	i32 399, ; 27
	i32 145, ; 28
	i32 398, ; 29
	i32 284, ; 30
	i32 18, ; 31
	i32 414, ; 32
	i32 246, ; 33
	i32 295, ; 34
	i32 310, ; 35
	i32 150, ; 36
	i32 104, ; 37
	i32 365, ; 38
	i32 95, ; 39
	i32 173, ; 40
	i32 339, ; 41
	i32 422, ; 42
	i32 395, ; 43
	i32 36, ; 44
	i32 244, ; 45
	i32 28, ; 46
	i32 279, ; 47
	i32 317, ; 48
	i32 50, ; 49
	i32 115, ; 50
	i32 70, ; 51
	i32 227, ; 52
	i32 369, ; 53
	i32 65, ; 54
	i32 170, ; 55
	i32 248, ; 56
	i32 145, ; 57
	i32 420, ; 58
	i32 174, ; 59
	i32 338, ; 60
	i32 364, ; 61
	i32 278, ; 62
	i32 313, ; 63
	i32 303, ; 64
	i32 40, ; 65
	i32 386, ; 66
	i32 89, ; 67
	i32 192, ; 68
	i32 81, ; 69
	i32 374, ; 70
	i32 66, ; 71
	i32 62, ; 72
	i32 86, ; 73
	i32 176, ; 74
	i32 277, ; 75
	i32 106, ; 76
	i32 410, ; 77
	i32 324, ; 78
	i32 102, ; 79
	i32 182, ; 80
	i32 35, ; 81
	i32 274, ; 82
	i32 432, ; 83
	i32 326, ; 84
	i32 228, ; 85
	i32 245, ; 86
	i32 366, ; 87
	i32 432, ; 88
	i32 394, ; 89
	i32 184, ; 90
	i32 119, ; 91
	i32 311, ; 92
	i32 406, ; 93
	i32 189, ; 94
	i32 424, ; 95
	i32 142, ; 96
	i32 141, ; 97
	i32 347, ; 98
	i32 187, ; 99
	i32 53, ; 100
	i32 35, ; 101
	i32 141, ; 102
	i32 271, ; 103
	i32 281, ; 104
	i32 194, ; 105
	i32 207, ; 106
	i32 375, ; 107
	i32 295, ; 108
	i32 8, ; 109
	i32 14, ; 110
	i32 428, ; 111
	i32 323, ; 112
	i32 51, ; 113
	i32 216, ; 114
	i32 352, ; 115
	i32 306, ; 116
	i32 136, ; 117
	i32 101, ; 118
	i32 288, ; 119
	i32 333, ; 120
	i32 116, ; 121
	i32 272, ; 122
	i32 163, ; 123
	i32 431, ; 124
	i32 217, ; 125
	i32 166, ; 126
	i32 67, ; 127
	i32 202, ; 128
	i32 406, ; 129
	i32 256, ; 130
	i32 80, ; 131
	i32 101, ; 132
	i32 385, ; 133
	i32 328, ; 134
	i32 213, ; 135
	i32 117, ; 136
	i32 259, ; 137
	i32 260, ; 138
	i32 411, ; 139
	i32 355, ; 140
	i32 340, ; 141
	i32 78, ; 142
	i32 339, ; 143
	i32 254, ; 144
	i32 114, ; 145
	i32 121, ; 146
	i32 48, ; 147
	i32 178, ; 148
	i32 240, ; 149
	i32 362, ; 150
	i32 128, ; 151
	i32 304, ; 152
	i32 275, ; 153
	i32 82, ; 154
	i32 110, ; 155
	i32 75, ; 156
	i32 350, ; 157
	i32 403, ; 158
	i32 262, ; 159
	i32 230, ; 160
	i32 53, ; 161
	i32 330, ; 162
	i32 199, ; 163
	i32 69, ; 164
	i32 329, ; 165
	i32 198, ; 166
	i32 83, ; 167
	i32 172, ; 168
	i32 426, ; 169
	i32 116, ; 170
	i32 188, ; 171
	i32 200, ; 172
	i32 156, ; 173
	i32 199, ; 174
	i32 269, ; 175
	i32 167, ; 176
	i32 322, ; 177
	i32 296, ; 178
	i32 205, ; 179
	i32 32, ; 180
	i32 228, ; 181
	i32 122, ; 182
	i32 72, ; 183
	i32 245, ; 184
	i32 62, ; 185
	i32 364, ; 186
	i32 257, ; 187
	i32 361, ; 188
	i32 161, ; 189
	i32 113, ; 190
	i32 390, ; 191
	i32 88, ; 192
	i32 226, ; 193
	i32 437, ; 194
	i32 253, ; 195
	i32 105, ; 196
	i32 18, ; 197
	i32 146, ; 198
	i32 118, ; 199
	i32 58, ; 200
	i32 290, ; 201
	i32 17, ; 202
	i32 52, ; 203
	i32 92, ; 204
	i32 400, ; 205
	i32 261, ; 206
	i32 243, ; 207
	i32 434, ; 208
	i32 378, ; 209
	i32 55, ; 210
	i32 129, ; 211
	i32 152, ; 212
	i32 41, ; 213
	i32 195, ; 214
	i32 92, ; 215
	i32 402, ; 216
	i32 194, ; 217
	i32 355, ; 218
	i32 334, ; 219
	i32 50, ; 220
	i32 404, ; 221
	i32 162, ; 222
	i32 13, ; 223
	i32 308, ; 224
	i32 386, ; 225
	i32 272, ; 226
	i32 329, ; 227
	i32 36, ; 228
	i32 67, ; 229
	i32 109, ; 230
	i32 378, ; 231
	i32 383, ; 232
	i32 221, ; 233
	i32 232, ; 234
	i32 273, ; 235
	i32 224, ; 236
	i32 99, ; 237
	i32 363, ; 238
	i32 99, ; 239
	i32 396, ; 240
	i32 11, ; 241
	i32 374, ; 242
	i32 192, ; 243
	i32 11, ; 244
	i32 220, ; 245
	i32 315, ; 246
	i32 25, ; 247
	i32 128, ; 248
	i32 76, ; 249
	i32 307, ; 250
	i32 109, ; 251
	i32 252, ; 252
	i32 333, ; 253
	i32 331, ; 254
	i32 106, ; 255
	i32 2, ; 256
	i32 26, ; 257
	i32 286, ; 258
	i32 157, ; 259
	i32 225, ; 260
	i32 430, ; 261
	i32 267, ; 262
	i32 21, ; 263
	i32 433, ; 264
	i32 49, ; 265
	i32 43, ; 266
	i32 126, ; 267
	i32 276, ; 268
	i32 368, ; 269
	i32 59, ; 270
	i32 119, ; 271
	i32 336, ; 272
	i32 372, ; 273
	i32 299, ; 274
	i32 285, ; 275
	i32 3, ; 276
	i32 179, ; 277
	i32 305, ; 278
	i32 325, ; 279
	i32 38, ; 280
	i32 373, ; 281
	i32 124, ; 282
	i32 383, ; 283
	i32 388, ; 284
	i32 427, ; 285
	i32 325, ; 286
	i32 357, ; 287
	i32 243, ; 288
	i32 427, ; 289
	i32 137, ; 290
	i32 149, ; 291
	i32 85, ; 292
	i32 90, ; 293
	i32 370, ; 294
	i32 309, ; 295
	i32 438, ; 296
	i32 380, ; 297
	i32 367, ; 298
	i32 306, ; 299
	i32 384, ; 300
	i32 415, ; 301
	i32 281, ; 302
	i32 292, ; 303
	i32 337, ; 304
	i32 208, ; 305
	i32 342, ; 306
	i32 307, ; 307
	i32 133, ; 308
	i32 401, ; 309
	i32 96, ; 310
	i32 3, ; 311
	i32 423, ; 312
	i32 105, ; 313
	i32 426, ; 314
	i32 33, ; 315
	i32 154, ; 316
	i32 186, ; 317
	i32 158, ; 318
	i32 155, ; 319
	i32 398, ; 320
	i32 82, ; 321
	i32 353, ; 322
	i32 301, ; 323
	i32 143, ; 324
	i32 87, ; 325
	i32 19, ; 326
	i32 302, ; 327
	i32 51, ; 328
	i32 221, ; 329
	i32 255, ; 330
	i32 271, ; 331
	i32 370, ; 332
	i32 430, ; 333
	i32 61, ; 334
	i32 54, ; 335
	i32 4, ; 336
	i32 97, ; 337
	i32 264, ; 338
	i32 270, ; 339
	i32 17, ; 340
	i32 373, ; 341
	i32 181, ; 342
	i32 155, ; 343
	i32 84, ; 344
	i32 262, ; 345
	i32 29, ; 346
	i32 45, ; 347
	i32 64, ; 348
	i32 66, ; 349
	i32 233, ; 350
	i32 421, ; 351
	i32 172, ; 352
	i32 310, ; 353
	i32 1, ; 354
	i32 345, ; 355
	i32 253, ; 356
	i32 47, ; 357
	i32 210, ; 358
	i32 24, ; 359
	i32 278, ; 360
	i32 366, ; 361
	i32 185, ; 362
	i32 358, ; 363
	i32 385, ; 364
	i32 222, ; 365
	i32 197, ; 366
	i32 379, ; 367
	i32 397, ; 368
	i32 165, ; 369
	i32 108, ; 370
	i32 225, ; 371
	i32 12, ; 372
	i32 397, ; 373
	i32 304, ; 374
	i32 63, ; 375
	i32 27, ; 376
	i32 23, ; 377
	i32 93, ; 378
	i32 392, ; 379
	i32 168, ; 380
	i32 12, ; 381
	i32 349, ; 382
	i32 239, ; 383
	i32 185, ; 384
	i32 231, ; 385
	i32 29, ; 386
	i32 103, ; 387
	i32 14, ; 388
	i32 249, ; 389
	i32 126, ; 390
	i32 287, ; 391
	i32 319, ; 392
	i32 91, ; 393
	i32 308, ; 394
	i32 189, ; 395
	i32 371, ; 396
	i32 174, ; 397
	i32 266, ; 398
	i32 9, ; 399
	i32 362, ; 400
	i32 86, ; 401
	i32 365, ; 402
	i32 298, ; 403
	i32 331, ; 404
	i32 425, ; 405
	i32 71, ; 406
	i32 168, ; 407
	i32 1, ; 408
	i32 318, ; 409
	i32 5, ; 410
	i32 425, ; 411
	i32 44, ; 412
	i32 27, ; 413
	i32 376, ; 414
	i32 403, ; 415
	i32 346, ; 416
	i32 399, ; 417
	i32 158, ; 418
	i32 321, ; 419
	i32 112, ; 420
	i32 435, ; 421
	i32 215, ; 422
	i32 198, ; 423
	i32 121, ; 424
	i32 359, ; 425
	i32 367, ; 426
	i32 193, ; 427
	i32 191, ; 428
	i32 175, ; 429
	i32 336, ; 430
	i32 277, ; 431
	i32 247, ; 432
	i32 214, ; 433
	i32 159, ; 434
	i32 131, ; 435
	i32 341, ; 436
	i32 57, ; 437
	i32 356, ; 438
	i32 138, ; 439
	i32 83, ; 440
	i32 30, ; 441
	i32 288, ; 442
	i32 10, ; 443
	i32 190, ; 444
	i32 237, ; 445
	i32 353, ; 446
	i32 338, ; 447
	i32 171, ; 448
	i32 356, ; 449
	i32 215, ; 450
	i32 285, ; 451
	i32 150, ; 452
	i32 94, ; 453
	i32 196, ; 454
	i32 298, ; 455
	i32 238, ; 456
	i32 60, ; 457
	i32 395, ; 458
	i32 229, ; 459
	i32 392, ; 460
	i32 157, ; 461
	i32 410, ; 462
	i32 181, ; 463
	i32 207, ; 464
	i32 64, ; 465
	i32 391, ; 466
	i32 88, ; 467
	i32 79, ; 468
	i32 47, ; 469
	i32 227, ; 470
	i32 223, ; 471
	i32 143, ; 472
	i32 407, ; 473
	i32 201, ; 474
	i32 347, ; 475
	i32 292, ; 476
	i32 74, ; 477
	i32 91, ; 478
	i32 344, ; 479
	i32 361, ; 480
	i32 387, ; 481
	i32 135, ; 482
	i32 90, ; 483
	i32 330, ; 484
	i32 381, ; 485
	i32 350, ; 486
	i32 354, ; 487
	i32 289, ; 488
	i32 182, ; 489
	i32 256, ; 490
	i32 358, ; 491
	i32 405, ; 492
	i32 112, ; 493
	i32 42, ; 494
	i32 401, ; 495
	i32 159, ; 496
	i32 223, ; 497
	i32 384, ; 498
	i32 251, ; 499
	i32 251, ; 500
	i32 4, ; 501
	i32 103, ; 502
	i32 382, ; 503
	i32 70, ; 504
	i32 195, ; 505
	i32 381, ; 506
	i32 60, ; 507
	i32 39, ; 508
	i32 279, ; 509
	i32 211, ; 510
	i32 180, ; 511
	i32 153, ; 512
	i32 56, ; 513
	i32 34, ; 514
	i32 206, ; 515
	i32 229, ; 516
	i32 239, ; 517
	i32 372, ; 518
	i32 276, ; 519
	i32 21, ; 520
	i32 163, ; 521
	i32 342, ; 522
	i32 416, ; 523
	i32 340, ; 524
	i32 335, ; 525
	i32 140, ; 526
	i32 419, ; 527
	i32 208, ; 528
	i32 89, ; 529
	i32 147, ; 530
	i32 291, ; 531
	i32 260, ; 532
	i32 162, ; 533
	i32 320, ; 534
	i32 196, ; 535
	i32 6, ; 536
	i32 169, ; 537
	i32 31, ; 538
	i32 107, ; 539
	i32 301, ; 540
	i32 263, ; 541
	i32 212, ; 542
	i32 417, ; 543
	i32 335, ; 544
	i32 205, ; 545
	i32 219, ; 546
	i32 235, ; 547
	i32 183, ; 548
	i32 274, ; 549
	i32 328, ; 550
	i32 167, ; 551
	i32 302, ; 552
	i32 393, ; 553
	i32 359, ; 554
	i32 140, ; 555
	i32 413, ; 556
	i32 59, ; 557
	i32 242, ; 558
	i32 144, ; 559
	i32 242, ; 560
	i32 186, ; 561
	i32 236, ; 562
	i32 81, ; 563
	i32 190, ; 564
	i32 265, ; 565
	i32 74, ; 566
	i32 130, ; 567
	i32 25, ; 568
	i32 7, ; 569
	i32 93, ; 570
	i32 332, ; 571
	i32 137, ; 572
	i32 268, ; 573
	i32 219, ; 574
	i32 216, ; 575
	i32 113, ; 576
	i32 9, ; 577
	i32 248, ; 578
	i32 211, ; 579
	i32 263, ; 580
	i32 104, ; 581
	i32 19, ; 582
	i32 213, ; 583
	i32 300, ; 584
	i32 314, ; 585
	i32 438, ; 586
	i32 294, ; 587
	i32 33, ; 588
	i32 282, ; 589
	i32 46, ; 590
	i32 233, ; 591
	i32 389, ; 592
	i32 379, ; 593
	i32 418, ; 594
	i32 30, ; 595
	i32 283, ; 596
	i32 220, ; 597
	i32 57, ; 598
	i32 134, ; 599
	i32 114, ; 600
	i32 337, ; 601
	i32 377, ; 602
	i32 431, ; 603
	i32 348, ; 604
	i32 369, ; 605
	i32 55, ; 606
	i32 360, ; 607
	i32 209, ; 608
	i32 6, ; 609
	i32 77, ; 610
	i32 293, ; 611
	i32 183, ; 612
	i32 375, ; 613
	i32 111, ; 614
	i32 259, ; 615
	i32 297, ; 616
	i32 400, ; 617
	i32 266, ; 618
	i32 371, ; 619
	i32 102, ; 620
	i32 405, ; 621
	i32 419, ; 622
	i32 402, ; 623
	i32 377, ; 624
	i32 170, ; 625
	i32 115, ; 626
	i32 413, ; 627
	i32 332, ; 628
	i32 287, ; 629
	i32 76, ; 630
	i32 343, ; 631
	i32 85, ; 632
	i32 345, ; 633
	i32 433, ; 634
	i32 280, ; 635
	i32 255, ; 636
	i32 434, ; 637
	i32 417, ; 638
	i32 322, ; 639
	i32 160, ; 640
	i32 2, ; 641
	i32 293, ; 642
	i32 24, ; 643
	i32 273, ; 644
	i32 32, ; 645
	i32 117, ; 646
	i32 37, ; 647
	i32 16, ; 648
	i32 412, ; 649
	i32 52, ; 650
	i32 387, ; 651
	i32 415, ; 652
	i32 346, ; 653
	i32 191, ; 654
	i32 20, ; 655
	i32 267, ; 656
	i32 123, ; 657
	i32 154, ; 658
	i32 204, ; 659
	i32 300, ; 660
	i32 250, ; 661
	i32 131, ; 662
	i32 407, ; 663
	i32 282, ; 664
	i32 148, ; 665
	i32 193, ; 666
	i32 269, ; 667
	i32 120, ; 668
	i32 28, ; 669
	i32 132, ; 670
	i32 100, ; 671
	i32 352, ; 672
	i32 134, ; 673
	i32 390, ; 674
	i32 320, ; 675
	i32 153, ; 676
	i32 97, ; 677
	i32 125, ; 678
	i32 270, ; 679
	i32 69, ; 680
	i32 210, ; 681
	i32 72, ; 682
	i32 428, ; 683
	i32 305, ; 684
	i32 323, ; 685
	i32 409, ; 686
	i32 0, ; 687
	i32 250, ; 688
	i32 136, ; 689
	i32 124, ; 690
	i32 71, ; 691
	i32 111, ; 692
	i32 315, ; 693
	i32 202, ; 694
	i32 222, ; 695
	i32 152, ; 696
	i32 420, ; 697
	i32 436, ; 698
	i32 343, ; 699
	i32 238, ; 700
	i32 118, ; 701
	i32 291, ; 702
	i32 177, ; 703
	i32 437, ; 704
	i32 404, ; 705
	i32 127, ; 706
	i32 133, ; 707
	i32 203, ; 708
	i32 77, ; 709
	i32 46, ; 710
	i32 294, ; 711
	i32 73, ; 712
	i32 63, ; 713
	i32 98, ; 714
	i32 84, ; 715
	i32 421, ; 716
	i32 43, ; 717
	i32 61, ; 718
	i32 212, ; 719
	i32 321, ; 720
	i32 357, ; 721
	i32 200, ; 722
	i32 37, ; 723
	i32 40, ; 724
	i32 284, ; 725
	i32 349, ; 726
	i32 160, ; 727
	i32 257, ; 728
	i32 98, ; 729
	i32 368, ; 730
	i32 258, ; 731
	i32 289, ; 732
	i32 354, ; 733
	i32 264, ; 734
	i32 363, ; 735
	i32 203, ; 736
	i32 396, ; 737
	i32 261, ; 738
	i32 0, ; 739
	i32 135, ; 740
	i32 20, ; 741
	i32 65, ; 742
	i32 408, ; 743
	i32 125, ; 744
	i32 75, ; 745
	i32 313, ; 746
	i32 164, ; 747
	i32 204, ; 748
	i32 175, ; 749
	i32 156, ; 750
	i32 408, ; 751
	i32 235, ; 752
	i32 5, ; 753
	i32 416, ; 754
	i32 252, ; 755
	i32 49, ; 756
	i32 327, ; 757
	i32 409, ; 758
	i32 144, ; 759
	i32 139, ; 760
	i32 389, ; 761
	i32 100, ; 762
	i32 226, ; 763
	i32 123, ; 764
	i32 120, ; 765
	i32 142, ; 766
	i32 39, ; 767
	i32 68, ; 768
	i32 360, ; 769
	i32 41, ; 770
	i32 164, ; 771
	i32 73, ; 772
	i32 422, ; 773
	i32 165, ; 774
	i32 206, ; 775
	i32 127, ; 776
	i32 299, ; 777
	i32 348, ; 778
	i32 68, ; 779
	i32 169, ; 780
	i32 218, ; 781
	i32 312, ; 782
	i32 286, ; 783
	i32 179, ; 784
	i32 258, ; 785
	i32 231, ; 786
	i32 241, ; 787
	i32 319, ; 788
	i32 151, ; 789
	i32 45, ; 790
	i32 108, ; 791
	i32 244, ; 792
	i32 48, ; 793
	i32 96, ; 794
	i32 31, ; 795
	i32 23, ; 796
	i32 166, ; 797
	i32 22, ; 798
	i32 180, ; 799
	i32 138, ; 800
	i32 78, ; 801
	i32 429, ; 802
	i32 173, ; 803
	i32 54, ; 804
	i32 312, ; 805
	i32 317, ; 806
	i32 249, ; 807
	i32 254, ; 808
	i32 10, ; 809
	i32 275, ; 810
	i32 187, ; 811
	i32 316, ; 812
	i32 224, ; 813
	i32 303, ; 814
	i32 16, ; 815
	i32 436, ; 816
	i32 382, ; 817
	i32 388, ; 818
	i32 139, ; 819
	i32 217, ; 820
	i32 13, ; 821
	i32 15, ; 822
	i32 122, ; 823
	i32 87, ; 824
	i32 149, ; 825
	i32 236, ; 826
	i32 22, ; 827
	i32 240, ; 828
	i32 34, ; 829
	i32 394, ; 830
	i32 79, ; 831
	i32 414, ; 832
	i32 341, ; 833
	i32 429, ; 834
	i32 234, ; 835
	i32 147, ; 836
	i32 241, ; 837
	i32 80, ; 838
	i32 412, ; 839
	i32 177, ; 840
	i32 265, ; 841
	i32 188, ; 842
	i32 176, ; 843
	i32 268, ; 844
	i32 391, ; 845
	i32 424, ; 846
	i32 42, ; 847
	i32 247, ; 848
	i32 26, ; 849
	i32 435, ; 850
	i32 314, ; 851
	i32 311, ; 852
	i32 107, ; 853
	i32 110, ; 854
	i32 214, ; 855
	i32 7, ; 856
	i32 376, ; 857
	i32 344, ; 858
	i32 44, ; 859
	i32 351, ; 860
	i32 161, ; 861
	i32 197, ; 862
	i32 148, ; 863
	i32 423, ; 864
	i32 280, ; 865
	i32 316, ; 866
	i32 38, ; 867
	i32 15, ; 868
	i32 146, ; 869
	i32 8, ; 870
	i32 297, ; 871
	i32 318, ; 872
	i32 130, ; 873
	i32 418, ; 874
	i32 246, ; 875
	i32 334, ; 876
	i32 94 ; 877
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

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
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
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
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
