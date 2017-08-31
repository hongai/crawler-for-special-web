using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace win_csharp
{
	[StructLayout(LayoutKind.Explicit)] 
	public struct result_t
	{ 
		[FieldOffset(0)] public int start; 
		[FieldOffset(4)] public int length;
        [FieldOffset(8)] public int sPos1;
        [FieldOffset(12)] public int sPos2;
        [FieldOffset(16)] public int sPos3;
        [FieldOffset(20)] public int sPos4;
        [FieldOffset(24)] public int sPos5;
        [FieldOffset(28)] public int sPos6;
        [FieldOffset(32)] public int sPos7;
        [FieldOffset(36)] public int sPos8;
        [FieldOffset(40)] public int sPos9;
        [FieldOffset(44)] public int sPos10;
       //[FieldOffset(12)] public int sPosLow;
        [FieldOffset(48)] public int POS_id; 
		[FieldOffset(52)] public int word_ID;
        [FieldOffset(56)] public int word_type;
        [FieldOffset(60)] public int weight;
	}
	class Class1
	{
        const string path = @"E:\safe's crawler\test\test\bin\Debug\NLPIR.dll";//设定dll的路径
        //对函数进行申明
        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_Init")]
		public static extern bool NLPIR_Init(String sInitDirPath,int encoding,String sLicenseCode);

        //特别注意，C语言的函数NLPIR_API const char * NLPIR_ParagraphProcess(const char *sParagraph,int bPOStagged=1);必须对应下面的申明
        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_ParagraphProcess")]
        public static extern IntPtr NLPIR_ParagraphProcess(String sParagraph, int bPOStagged = 1);
	
        [DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_Exit")]
		public static extern bool NLPIR_Exit();
	
		[DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_ImportUserDict")]
		public static extern int NLPIR_ImportUserDict(String sFilename);

		[DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_FileProcess")]
		public static extern bool NLPIR_FileProcess(String sSrcFilename,String sDestFilename,int bPOStagged=1);
		
		[DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_FileProcessEx")]
		public static extern bool NLPIR_FileProcessEx(String sSrcFilename,String sDestFilename);
		
		[DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_GetParagraphProcessAWordCount")]
		static extern int NLPIR_GetParagraphProcessAWordCount(String sParagraph);
		//NLPIR_GetParagraphProcessAWordCount
		[DllImport(path,CharSet=CharSet.Ansi,EntryPoint="NLPIR_ParagraphProcessAW")]
		static extern void NLPIR_ParagraphProcessAW(int nCount, [Out,MarshalAs(UnmanagedType.LPArray)] result_t[] result);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_AddUserWord")]
        static extern int NLPIR_AddUserWord(String sWord);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_SaveTheUsrDic")]
        static extern int NLPIR_SaveTheUsrDic();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_DelUsrWord")]
        static extern int NLPIR_DelUsrWord(String sWord);

       [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Start")]
        static extern bool NLPIR_NWI_Start();

       [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Complete")]
        static extern bool NLPIR_NWI_Complete();

       [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddFile")]
        static extern bool NLPIR_NWI_AddFile(String sText);

       [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddMem")]
        static extern bool NLPIR_NWI_AddMem(String sText);

       [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_NWI_GetResult")]
      public static extern IntPtr NLPIR_NWI_GetResult(bool bWeightOut = false);

      [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Result2UserDict")]
        static extern uint NLPIR_NWI_Result2UserDict();
    
        [DllImport(path, CharSet = CharSet.Ansi,CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_GetKeyWords")]
       public static extern IntPtr NLPIR_GetKeyWords(String sText,int nMaxKeyLimit=50,bool bWeightOut=false);

        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_GetFileKeyWords")]
        public static extern IntPtr NLPIR_GetFileKeyWords(String sFilename, int nMaxKeyLimit = 50, bool bWeightOut = false);
		[STAThread]
		static void Main(string[] args)
		{
           
            if (!NLPIR_Init(@"E:\safe's crawler\test\test", 0,""))//给出Data文件所在的路径，注意根据实际情况修改。
			{
				System.Console.WriteLine("Init ICTCLAS failed!");
				return;
			}
            System.Console.WriteLine("Init ICTCLAS success!");
            StreamReader sr = new StreamReader(@"E:\safe's crawler\test\test\bin\Debug\format.txt", Encoding.UTF8);

            String s = sr.ReadToEnd();
            sr.Close();
            int count = NLPIR_GetParagraphProcessAWordCount(s);//先得到结果的词数
            System.Console.WriteLine("NLPIR_GetParagraphProcessAWordCount success!");

            result_t[] result = new result_t[count];//在客户端申请资源
			NLPIR_ParagraphProcessAW(count,result);//获取结果存到客户的内存中
            int i=1;
            foreach(result_t r in result)
            {
                String sWhichDic="";
                switch (r.word_type)
                {
                    case 0:
                        sWhichDic = "核心词典";
                        break;
                    case 1:
                        sWhichDic = "用户词典";
                        break;
                    case 2:
                        sWhichDic = "专业词典";
                        break;
                    default:
                        break;
                }
           }
          StringBuilder sResult = new StringBuilder(600); 
            //准备存储空间         
   
          IntPtr intPtr =NLPIR_ParagraphProcess(s);//切分结果保存为IntPtr类型
          String str = Marshal.PtrToStringAnsi(intPtr);//将切分结果转换为string
          Console.WriteLine(str);
            StreamWriter sw = new StreamWriter(@"E:\safe's crawler\test\test\bin\Debug\participle.txt");
            sw.Write(str);
            sw.Close();


        }
	}
}
