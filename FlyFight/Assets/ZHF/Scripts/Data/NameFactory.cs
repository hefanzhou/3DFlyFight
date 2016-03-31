using System.Collections;
using UnityEngine;
public class NameFactory
{
    const string allLastNameString = @"依凝|如柏|雁菱|凝竹|宛白|初柔|南蕾|书萱|梦槐|南琴|绿海|沛儿|晓瑶|凝蝶|紫雪|念双|念真|曼寒|凡霜|飞雪|雪兰|雅霜|从蓉|冷雪|靖巧|翠丝|觅翠|凡白|乐蓉|迎波|丹烟|梦旋|书双|念桃|夜天|安筠|觅柔|初南|秋蝶|千易|安露|诗蕊|山雁|友菱|香露|晓兰|白卉|语山|冷珍|秋翠|夏柳|如之|忆南|书易|翠桃|寄瑶|如曼|问柳|幻桃|又菡|醉蝶|亦绿|诗珊|听芹|新之|易巧|念云|晓灵|静枫|夏蓉|如南|幼丝|秋白|冰安|秋白|南风|醉山|初彤|凝海|紫文|凌晴|雅琴|傲安|傲之|初蝶|代芹|诗霜|碧灵|诗柳|夏柳|采白|慕梅|乐安|冬菱|紫安|宛凝|雨雪|易真|安荷|静竹|代柔|丹秋|绮梅|依白|凝荷|幼珊|忆彤|凌青|之桃|芷荷|听荷|代玉|念珍|梦菲|夜春|千秋|白秋|谷菱|飞松|初瑶|惜灵|梦易|新瑶|曼梅|碧曼|友瑶|雨兰|夜柳|芷珍|含芙|夜云|依萱|凝雁|以莲|安南|幼晴|尔琴|飞阳|";
    const string allFirstNameString = @"欧阳|太史|端木|上官|司马|东方|独孤|南宫|万俟|闻人|夏侯|诸葛|尉迟|公羊|赫连|澹台|皇甫|宗政|濮阳|公冶|太叔|申屠|公孙|慕容|仲孙|钟离|长孙|宇文|司徒|鲜于|司空|闾丘|子车|亓官|司寇|巫马|公西|颛孙|壤驷|公良|漆雕|乐正|宰父|谷梁|拓跋|夹谷|轩辕|令狐|段干|百里|呼延|东郭|南门|羊舌|微生|公户|公玉|公仪|梁丘|公仲|公上|公门|公山|公坚|左丘|公伯|西门|公祖|第五|公乘|贯丘|公皙|南荣|东里|东宫|仲长|子书|子桑|即墨|达奚|褚师|刘观|刘傅|康有|东门";
    static string[] lastNameArray = allLastNameString.Split('|');
    static string[] fristNameArray = allFirstNameString.Split('|');
    public static string RandomGetName()
    {
        return fristNameArray[Random.Range(0, fristNameArray.Length)] + lastNameArray[Random.Range(0, lastNameArray.Length)];
    }
}
