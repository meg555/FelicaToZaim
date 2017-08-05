// ============================================================================
// 
// Linq ユーティリティクラス
// Copyright (C) 2015 by SHINTA
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;

namespace Shinta
{
	public class LinqUtils
	{
		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// データベースファイル内に、oTable で示される型に対応したテーブルを作成する
		// ＜引数＞ oCmd: 対象となるデータベースファイルに接続された状態の DbCommand
		//          oTable: テーブルの型を定義したクラスのインスタンス
		//          oUniques: ユニーク制約を付けるフィールド名（複数キーで 1 つのユニークにする場合はカンマ区切りで 1 つの String とする）
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		public static void CreateTable(DbCommand oCmd, Type oTypeOfTable, List<String> oUniques = null, String oAutoIncrement = null)
		{
			StringBuilder aCmdText = new StringBuilder();
			aCmdText.Append("CREATE TABLE IF NOT EXISTS " + TableName(oTypeOfTable) + "(");
			IEnumerable<ColumnAttribute> aAttrs = oTypeOfTable.GetProperties().Select(x => Attribute.GetCustomAttribute(x, typeof(ColumnAttribute)) as ColumnAttribute);
			if (aAttrs.Count() == 0)
			{
				throw new Exception("テーブルにフィールドが存在しません。");
			}

			// フィールド
			foreach (ColumnAttribute aFieldAttr in aAttrs)
			{
				// フィールド名
				aCmdText.Append(aFieldAttr.Name);

				// 型
				if (aFieldAttr.DbType == "INT")
				{
					// INT は INTEGER にする（SQLite の AUTOINCREMENT でのエラー回避）
					aCmdText.Append(" INTEGER");
				}
				else
				{
					aCmdText.Append(" " + aFieldAttr.DbType);
				}

				// NOT NULL
				if (!aFieldAttr.CanBeNull)
				{
					aCmdText.Append(" NOT NULL");
				}

				// 主キー
				if (aFieldAttr.IsPrimaryKey)
				{
					aCmdText.Append(" PRIMARY KEY");
				}

				// オートインクリメント
				if (!String.IsNullOrEmpty(oAutoIncrement) && aFieldAttr.Name == oAutoIncrement)
				{
					aCmdText.Append(" AUTOINCREMENT");
				}

				aCmdText.Append(",");
			}

			// ユニーク制約
			if (oUniques != null)
			{
				foreach (String aUnique in oUniques)
				{
					aCmdText.Append("UNIQUE(");
					String[] aUniqueSplit = aUnique.Split(',');
					foreach (String aElement in aUniqueSplit)
					{
						aCmdText.Append(aElement + ",");
					}
					aCmdText.Remove(aCmdText.Length - 1, 1);
					aCmdText.Append("),");
				}
			}

			aCmdText.Remove(aCmdText.Length - 1, 1);
			aCmdText.Append(");");

			// テーブル作成
			oCmd.CommandText = aCmdText.ToString();
			oCmd.ExecuteNonQuery();

		}

		// --------------------------------------------------------------------
		// データベースファイル内のテーブルにインデックスを作成
		// --------------------------------------------------------------------
		public static void CreateIndex(DbCommand oCmd, Type oTypeOfTable, List<String> oIndices)
		{
			if (oIndices == null)
			{
				return;
			}
			foreach (String aIndex in oIndices)
			{
				oCmd.CommandText = "CREATE INDEX IF NOT EXISTS index_" + aIndex + " ON " + TableName(oTypeOfTable) + "(" + aIndex + ");";
				oCmd.ExecuteNonQuery();
			}
		}

		// --------------------------------------------------------------------
		// テーブル名
		// ＜引数＞ oTypeOfTable: typeof(テーブル定義クラス名) or aInstance.GetType()
		// ＜例外＞ Exception（Name 属性の無いクラスを指定した場合など）
		// --------------------------------------------------------------------
		public static String TableName(Type oTypeOfTable)
		{
			return (Attribute.GetCustomAttribute(oTypeOfTable, typeof(TableAttribute)) as TableAttribute).Name;
		}

	} // LinqUtils

} // namespace Shinta

