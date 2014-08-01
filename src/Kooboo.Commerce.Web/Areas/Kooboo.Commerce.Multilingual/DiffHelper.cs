using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public static class DiffHelper
    {
        public static string GetDiffHtml(string oldText, string newText)
        {
            var html = new StringBuilder();
            var diff = new InlineDiffBuilder(new Differ()).BuildDiffModel(oldText ?? String.Empty, newText ?? String.Empty);
            
            if (diff.Lines.Count > 0)
            {
                html.Append("<div class=\"diff-panel\">");

                foreach (var piece in diff.Lines)
                {
                    html.Append("<span class=\"diff-line\">");
                    WritePiece(html, piece);
                    html.Append("</span>");
                }

                html.Append("</div>");
            }

            return html.ToString();
        }

        static void WritePiece(StringBuilder html, DiffPiece piece)
        {
            if (piece.Type == DiffPlex.DiffBuilder.Model.ChangeType.Unchanged)
            {
                WriteSpan(html, "diff-unchanged", piece.Text);
            }
            else if (piece.Type == DiffPlex.DiffBuilder.Model.ChangeType.Inserted)
            {
                WriteSpan(html, "diff-inserted", piece.Text);
            }
            else if (piece.Type == DiffPlex.DiffBuilder.Model.ChangeType.Deleted)
            {
                WriteSpan(html, "diff-deleted", piece.Text);
            }
            else if (piece.Type == DiffPlex.DiffBuilder.Model.ChangeType.Modified)
            {
                html.Append("<span class=\"diff-modified\">");

                foreach (var subpiece in piece.SubPieces)
                {
                    WritePiece(html, subpiece);
                }

                html.Append("</span>");
            }
        }

        static void WriteSpan(StringBuilder html, string className, string text)
        {
            if (String.IsNullOrEmpty(className))
            {
                html.AppendFormat("<span>{0}</span>", text);
            }
            else
            {
                html.AppendFormat("<span class=\"{0}\">{1}</span>", className, text);
            }
        }
    }
}