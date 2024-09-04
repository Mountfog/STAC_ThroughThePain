using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVParser
{
    // sPathName :  폴더가 Resources 하위 부터 시작해야 한다. "TableData/test" 
    public static List<string[]> Load(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        //TextAsset kTextAsset2 = Resources.Load(sPathName, typeof(TextAsset)) as TextAsset;
        //TextAsset kTextAsset3 = (TextAsset)Resources.Load(sPathName, typeof(TextAsset));
        TextAsset kTextAsset = Resources.Load<TextAsset>(sPathName);

        if (kTextAsset == null)
            return null;

        StringReader _reader = new StringReader(kTextAsset.text);

        string inputData = _reader.ReadLine();
        while (inputData != null)
        {
            string[] datas = inputData.Split('\t');
            if (datas.Length == 0)
                continue;

            listData.Add(datas);

            inputData = _reader.ReadLine();
        }
        _reader.Close();

        return listData;
    }


    // sPathName :  주의) 폴더가 Assets부터 시작해야 한다. "Assets/Resources/TableData/test.csv" 
    public static List<string[]> Load2(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        StreamReader sr = new StreamReader(sPathName, Encoding.UTF8);
        if (sr == null) return null;

        while (!sr.EndOfStream)                         // 스트림의 끝까지 읽기
        {
            string line = sr.ReadLine();                // 한 줄씩 읽어온다.

            string[] datas = line.Split('\t');           // 탭을 기준으로 문자를 분리한다.
            if (datas.Length == 0)
                continue;

            listData.Add(datas);
        }
        sr.Close();

        return listData;
    }


    // sPathName :  주의) 폴더가 Assets부터 시작해야 한다. "Assets/Resources/TableData/test.csv" 
    public static List<string[]> Load3(string sPathName)
    {
        List<string[]> listData = new List<string[]>();

        //StreamReader sr = new StreamReader(sPathName, Encoding.UTF8);
        //if (sr == null) return null;
        //string strAll = sr.ReadToEnd();
        //sr.Close();

        string strAll = File.ReadAllText(sPathName, Encoding.UTF8);

        StringReader _reader = new StringReader(strAll);

        string inputData = _reader.ReadLine();
        while (inputData != null)
        {
            string[] datas = inputData.Split('\t');
            if (datas.Length == 0)
                continue;

            listData.Add(datas);

            inputData = _reader.ReadLine();
        }
        _reader.Close();
        return listData;
    }

}
