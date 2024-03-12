
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using Church.Data;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Church.Docs
{
    public static class TechnicalDocumentation
    {
        public static Response GenerateDoc(string sAssembly, string Root, string Folder)
        {
            Response Response = new Response();
            Response.Result = Result.OK;

            Assembly Assembly = System.Reflection.Assembly.Load(sAssembly);
            if (Assembly != null)
            {
                string FullPath = Assembly.GetFullPath(Root, Folder);
                string[] Files = Directory.GetFiles(FullPath, "*.CS");
                List<MethodDocumentation> Doc = new List<MethodDocumentation>();

                for (int i = 0; i < Files.Length; i++)
                {
                    List<MethodProperties> MProperties = new List<MethodProperties>();
                    var Type = Assembly.GetType(Files[i], Folder);

                    if (Type == null && Folder != null)
                    {
                        Type = Assembly.GetType(Files[i], string.Empty);
                    }

                    if (Type != null)
                    {
                        List<MethodInfo> Methods = Type.GetCurrentMethods();
                        foreach (MethodInfo Method in Methods)
                        {
                            var Attributes = Method.GetCustomAttributes(false);
                            var Attributes2= Method.GetCustomAttributes(true);

                            MethodDescription Description = (MethodDescription)Attributes.Where(a => a.GetType().Equals(typeof(MethodDescription))).FirstOrDefault();

                            var parameterDescriptions = string.Join(", ", Method.GetParameters()
                                     .Select(x => x.GetFormattedParameter())
                                     .ToArray());

                            MProperties.Add(new MethodProperties() { Description = Description != null ? Description.Description : "N/A", Input = parameterDescriptions, MethodName = Method.Name, Output = Method.ReturnType.Name });

                        }
                        Doc.Add(new MethodDocumentation() { ClassName = Type.Name, Properties = MProperties });
                    }
                }
                GenerateTable(Doc, FullPath);
            }
            return Response;

        }

        public static void GenerateTable(List<MethodDocumentation> Doc, string Path)
        {
            if (Doc.Count > 0)
            {
                // Create a document.
                using (DocX document = DocX.Create(Path + @"\\TDoc.docx"))
                {
                    foreach (MethodDocumentation MD in Doc)
                    {
                        // Add a title
                        document.InsertParagraph(MD.ClassName).FontSize(15d).SpacingAfter(10d).Alignment = Alignment.left;

                        // Add a Table into the document and sets its values.
                        var t = document.AddTable(MD.Properties.Count + 2, 4);
                        t.Design = TableDesign.ColorfulListAccent3;
                        t.Alignment = Alignment.center;

                        t.Rows[0].MergeCells(0, 3);
                        t.Rows[0].Cells[0].Paragraphs[0].Append("METODOS");

                        t.Rows[1].Cells[0].Paragraphs[0].Append("Nombre");
                        t.Rows[1].Cells[1].Paragraphs[0].Append("Entrada");
                        t.Rows[1].Cells[2].Paragraphs[0].Append("Salida");
                        t.Rows[1].Cells[3].Paragraphs[0].Append("Descripción");

                        int count = 2;
                        foreach (MethodProperties MP in MD.Properties)
                        {
                            t.Rows[count].Cells[0].Paragraphs[0].Append(MP.MethodName);
                            t.Rows[count].Cells[1].Paragraphs[0].Append(MP.Input);
                            t.Rows[count].Cells[2].Paragraphs[0].Append(MP.Output);
                            t.Rows[count].Cells[3].Paragraphs[0].Append(MP.Description);
                            count++;
                        }

                        document.InsertTable(t);

                    }
                    document.Save();
                }
            }
        }
    }
}
