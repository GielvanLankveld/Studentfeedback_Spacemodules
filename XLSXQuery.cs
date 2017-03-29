/**
 * Copyright (C) 2016 Open University (http://www.ou.nl/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentFeedback_SpaceModules
{
    class XLSXQuery
    {
        private List<Record> LoadFromXLSX(String fileName, String targetSheet)
        {
            //Prep the list
            List<Record> inputList = new List<Record>();

            //Get the records from the excel fileName's target sheet
            var book = new LinqToExcel.ExcelQueryFactory(fileName);

            var query =
                from row in book.Worksheet(targetSheet)
                let item = new
                {
                    Code = row["Code"].Cast<string>(),
                    Supplier = row["Supplier"].Cast<string>(),
                    Ref = row["Ref"].Cast<string>(),
                }
                where item.Supplier == "Walmart"
                select item;


            //Output the result
            return inputList;
        }
    }
}
