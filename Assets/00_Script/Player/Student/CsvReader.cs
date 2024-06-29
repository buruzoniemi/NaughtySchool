/*
 * Script to read Csv format files
 * Assign to a dedicated mischief type
 * Created by Misora Tanaka
 * 
 * date: 24/02/15
 * --- Log ---
 * 02/15: Change comments to English
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CsvReader
{
    /// <summary>
    /// Reads Csv files from the resources folder,
	/// converts them to Naughty's list and returns them
    /// </summary>
    /// <returns>Loaded Naughty list data</returns>
    public List<Mischief> SetCsvData()
    {
        // Temporary data initialization
        Mischief mischief = new Mischief();     
        mischief.commandList = new List<String>();
        // number of lines
        int height = 0;
        // List initialization
        List<Mischief> mischiefList = new List<Mischief>();
        List<string[]> csvList = new List<string[]>();

        // Temporary storage of Csv files
        TextAsset csvFile = Resources.Load("MischefCommandList") as TextAsset;
        // Convert Text Asset to StringReader
        StringReader reader = new StringReader(csvFile.text);
        
        // Don't load the first line
        string str = reader.ReadLine();

        // Read through to the end of a line
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();    // Read one line at a time
            csvList.Add(line.Split(','));       // Add to list separated by commas
            height++;                           // Add the number of lines
        }

        // Set the read data to the list
        for (int mi = 0; mi < height; mi++)
        {
            mischief.name = csvList[mi][0];                         
            mischief.commandNum = Convert.ToInt32(csvList[mi][1]);
            // Set key bindings by number of commands
            for (int ci = 0; ci < mischief.commandNum; ci++)
            {
                mischief.commandList.Add(new String(csvList[mi][ci + 2]));
            }
            mischief.isPlay = false;
            mischief.isComp = false;

			// Add to list
			mischiefList.Add(mischief);
            // Reset temporary data
            mischief = new Mischief();
            mischief.commandList = new List<String>();
        }

        // Returns a completed list
        return mischiefList;
    }
}
