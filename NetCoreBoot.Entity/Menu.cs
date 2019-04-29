////////////////////////////////////////////////////////////////////
//                          _ooOoo_                               //
//                         o8888888o                              //
//                         88" . "88                              //
//                         (| ^_^ |)                              //
//                         O\  =  /O                              //
//                      ____/`---'\____                           //
//                    .'  \\|     |//  `.                         //
//                   /  \\|||  :  |||//  \                        //
//                  /  _||||| -:- |||||-  \                       //
//                  |   | \\\  -  /// |   |                       //
//                  | \_|  ''\---/''  |   |                       //
//                  \  .-\__  `-`  ___/-. /                       //
//                ___`. .'  /--.--\  `. . ___                     //
//              ."" '<  `.___\_<|>_/___.'  >'"".                  //
//            | | :  `- \`.;`\ _ /`;.`/ - ` : | |                 //
//            \  \ `-.   \_ __\ /__ _/   .-` /  /                 //
//      ========`-.____`-.___\_____/___.-`____.-'========         //
//                           `=---='                              //
//      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //
//                   佛祖保佑       永不宕机     永无BUG          //
////////////////////////////////////////////////////////////////////

/**
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreBoot.Entity
{
	/// <summary>
	/// </summary>
	public partial class Menu
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		/// 父菜单ID
		/// </summary>
		[Required]
		[MaxLength(10)]
		public Int32 ParentId {get;set;}

		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[MaxLength(32)]
		public String Name {get;set;}

		/// <summary>
		/// 显示名称
		/// </summary>
		[MaxLength(128)]
		public String DisplayName {get;set;}

		/// <summary>
		/// 图标地址
		/// </summary>
		[MaxLength(128)]
		public String IconUrl {get;set;}

		/// <summary>
		/// 链接地址
		/// </summary>
		[MaxLength(128)]
		public String LinkUrl {get;set;}

		/// <summary>
		/// 排序数字
		/// </summary>
		[MaxLength(10)]
		public Int32? Sort {get;set;}

		/// <summary>
		/// 操作权限（按钮权限时使用）
		/// </summary>
		[MaxLength(256)]
		public String Permission {get;set;}

		/// <summary>
		/// 是否显示
		/// </summary>
		[Required]
		[MaxLength(1)]
		public Boolean IsDisplay {get;set;}

		/// <summary>
		/// 是否系统默认
		/// </summary>
		[Required]
		[MaxLength(1)]
		public Boolean IsSystem {get;set;}

		/// <summary>
		/// 添加人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public Int32 AddManagerId {get;set;}

		/// <summary>
		/// 添加时间
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime AddTime {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(10)]
		public Int32? ModifyManagerId {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? ModifyTime {get;set;}

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		[MaxLength(1)]
		public Boolean IsDelete {get;set;}


	}
}
