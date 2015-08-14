using System;
using System.Collections.Generic;
using System.Net.Mime;
using NHibernate;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sessionFactory = NHibernateConfig.CreateSessionFactory(true);
            var session = sessionFactory.OpenSession();

            CreateInitData(session);

            session.Flush();
            session.Close();

            Console.WriteLine("Finished Creating DB!");
            Console.ReadKey();
        }

        private static void CreateInitData(ISession session)
        {
            var MTA = new University
            {
                Acronyms = "MTA",
                Name = "המכללה האקדמית תל אביב יפו",
                SiteAddress = "www.mta.ac.il",
            };

            var BGU = new University
            {
                Acronyms = "BGU",
                Name = "אוניברסטית בן גוריון",
                SiteAddress = "in.bgu.ac.il/Pages/default.aspx",
            };


            const Faculty computersFaculty = Faculty.ComputerScience;
            const Faculty socialityFaculty = Faculty.SocietyPolitics;

            var romina = new Teacher("רומינה זיגדון", 232, "05x-xxxxxxx", "ROMINAZI@MTA.AC.IL", MTA);
            TeacherComment rominaComment = new TeacherComment
            {
                CommentText = "ממש עוזרת ללמוד",
                DateTime = DateTime.Now,
            };
            foreach (var teacherCritiria in TeacherComment.GetTeacherCommentCriterias())
            {
                rominaComment.CriteriaRatings.Add(new TeacherCriteriaRating(teacherCritiria, 5));
            }
            rominaComment.AddVote(new Vote(true));
            romina.AddTeacherCommnet(rominaComment);

            //Computer Sciences Teachers
            var oren = new Teacher("אורן איש שלום", 0, "", "ISHSHALO@MTA.AC.IL", BGU);
            var ofer = new Teacher("עופר אריאלי", 0, "", "OARIELI@MTA.AC.IL", BGU);
            var shlomit = new Teacher("שלומית אריאן", 0, "", "shlomita.mta@gmail.com", MTA);
            var baruch = new Teacher("אשל ברוך", 0, "", "BARUCHES@MTA.AC.IL", MTA);
            var hadar = new Teacher("הדר בינסקי", 0, "", "HBINSKY@MTA.AC.IL", MTA);
            var liron = new Teacher("לירון בלכר", 0, "", "LIRONBLE@MTA.AC.IL", MTA);
            var amirBen = new Teacher("אמיר בן עמרם", 0, "", "AMIRBEN@MTA.AC.IL", MTA);
            var michal = new Teacher("מיכל בן נח", 0, "", "BENNOAHM@MTA.AC.IL", MTA);
            var aharon = new Teacher("אהרון בן שפרוט", 0, "", "RONIBENS@MTA.AC.IL", MTA);
            var carmit = new Teacher("כרמית בנבנישתי שפילקה", 0, "", "CARMITBE@MTA.AC.IL", MTA);
            var yossi = new Teacher("יוסי בצלאל", 0, "", "YOSSI@MTA.AC.IL", MTA);
            var nili = new Teacher("נילי בק", 0, "", "BECKNILI@MTA.AC.IL", MTA);
            var omer = new Teacher("עמר ברקמן", 0, "", "OMER@MTA.AC.IL", MTA);
            var michalB = new Teacher("מיכל בריל", 0, "", "MICHALBR@MTA.AC.IL", MTA);
            var iris = new Teacher("איריס גבר רוזנבלום", 0, "", "GABER@MTA.AC.IL", MTA);
            var toma = new Teacher("טומב גבר", 0, "", "TOMAGEBE@MTA.AC.IL", MTA);
            var uri = new Teacher("אורי גלובוס", 0, "", "GLOBUS@MTA.AC.IL", MTA);
            var osnat = new Teacher("אסנת גניסלב", 0, "", "OSNATGEN@MTA.AC.IL", MTA);
            var gidon = new Teacher("גידעון דרור", 0, "", "GIDEON@MTA.AC.IL", MTA);
            var roi = new Teacher("רועי וגנר", 0, "", "RWAGNER@MTA.AC.IL", MTA);
            var rotem = new Teacher("רותם זילברברג", 0, "", "ROTEMZIL@MTA.AC.IL", MTA);
            var ishai = new Teacher("ישי חביב", 0, "", "ISHAYHAV@MTA.AC.IL", MTA);
            var uziel = new Teacher("עוזיאל חדד", 0, "", "UZIHADAD@MTA.AC.IL", MTA);
            var arieh = new Teacher("אריה חינקיס", 0, "", "ARIEHI@@MTA.AC.IL", MTA);
            var wissam = new Teacher("וויסאם חלילי", 0, "", "WISSAMKH@MTA.AC.IL", MTA);
            var lubov = new Teacher("ליובוב טטרואשוילי", 0, "", "LUBOVTE@MTA.AC.IL", MTA);
            var shmuel = new Teacher("שמואל טישברוביץ", 0, "", "TYSHBE@MTA.AC.IL", MTA);
            var hilel = new Teacher("הילל טל אלעזר", 0, "", "HILLEL@MTA.AC.IL", MTA);
            var guy = new Teacher("גיא יוסקוביץ", 0, "", "GUYYO@MTA.AC.IL", MTA);
            var boaz = new Teacher("בועז כהן", 0, "", "BOAZC@MTA.AC.IL", MTA);
            var ariehLev = new Teacher("אריה לב", 0, "03-9225106", "ARIEH@MTA.AC.IL", MTA);
            var dani = new Teacher("דני לוי", 0, "", "DANLEVY@MTA.AC.IL", MTA);
            var borisL = new Teacher("בוריס לוין", 0, "", "BORISLEV@MTA.AC.IL", MTA);
            var davidM = new Teacher("דוד מובוביץ", 0, "", "DAVIDMOV@MTA.AC.IL", MTA);
            var tzvi = new Teacher("צבי מלמד", 0, "", "TZVIME@MTA.AC.IL", MTA);
            var carmi = new Teacher("כרמי מרימוביץ", 0, "", "CARMI@MTA.AC.IL", MTA);
            var alonN = new Teacher("אלון נאור", 0, "", "ALONNA@MTA.AC.IL", MTA);
            var hilaN = new Teacher("הילה נעמן", 0, "", "HILANA@MTA.AC.IL", MTA);
            var zef = new Teacher("זף סגל", 0, "", "ZEF@MTA.AC.IL", MTA);
            var raid = new Teacher("ראיד סעאבנה", 0, "", "RAIDSA@MTA.AC.IL", MTA);
            var alex = new Teacher("אלכסנדר ספיבק", 0, "", "no mail yet", MTA);
            var adiA = new Teacher("עדי עקביה", 0, "", "AKAVIA@MTA.AC.IL", MTA);
            var avishaiF = new Teacher("אבישי פישמן", 0, "", "AVISHAIF@MTA.AC.IL", MTA);
            var doritP = new Teacher("דורית פרנס", 0, "", "DORITP@MTA.AC.IL", MTA);
            var alexComan = new Teacher("אלכס קומן", 0, "", "COMAN@MTA.AC.IL", MTA);
            var ilanK = new Teacher("אילן קירש", 0, "03-6803412", "KIRSH@MTA.AC.IL", MTA);
            var amirK = new Teacher("אמיר קירש", 0, "03-6803412", "AMIRK@MTA.AC.IL", MTA);
            var barakK = new Teacher("ברק קנדל", 0, "", "KANDELLB@MTA.AC.IL", MTA);
            var gilK = new Teacher("גיל קפלן", 0, "", "GILK68@GMAIL.COM", MTA);
            var roditi = new Teacher("יהודה רודיטי", 0, "03-6407969", "JR@MTA.AC.IL", MTA);
            var guyRonen = new Teacher("גיא רונן", 0, "", "GUYRONEN@MTA.AC.IL", MTA);
            var leonidR = new Teacher("לאוניד רסקין", 0, "", "LEONIDRA@MTA.AC.IL", MTA);
            var adiShabat = new Teacher("עדי שבת", 0, "", "ADISHABA@MTA.AC.IL", MTA);
            var ester = new Teacher("אסתר שטיין", 0, "", "ESTERST@MTA.AC.IL", MTA);
            var heli = new Teacher("חלי שלסקי", 0, "", "chelishe@mta.ac.il", MTA);
            var adiShrivman= new Teacher("עדי שרייבמן", 0, "", "ADISH@MTA.AC.IL", MTA);
            
            //Behaviour
            var avidar = new Teacher("אבידר גלית", 0, "", "GALITAVI@MTA.AC.IL", MTA);
            var ronitA = new Teacher("רונית אביצור חמיאל", 0, "", "AVITSUR@MTA.AC.IL", MTA);
            var sabrina = new Teacher("סברינה אופנהיימר", 0, "", "SABRINA@MTA.AC.IL", MTA);
            var michalOr = new Teacher("מיכל אורגלר שוב", 0, "", "MICHALOR@MTA.AC.IL", MTA);
            var aviA = new Teacher("אבי אללוף", 0, "02-6754743", "AVI@MTA.AC.IL", MTA);
            var odeliaE = new Teacher("אודליה אלקנה", 0, "", "ELKANA@MTA.AC.IL", MTA);
            var HaimA = new Teacher("חיים אמסל", 0, "", "JAIMEAMS@MTA.AC.IL", MTA);
            var sofiaA = new Teacher("סופיה אסטנשקו", 0, "", "SOFIAAS@MTA.AC.IL", MTA);
            var shaiB = new Teacher("שי בנבנישתי", 0, "", "BENVENIS@MTA.AC.IL", MTA);
            var hedvaB = new Teacher("חדוה בראונשטיין ברקו", 0, "", "HEDVAB@MTA.AC.IL", MTA);
            var dafnaB = new Teacher("דפנה ברגרבסט אגסי", 0, "", "DAFNABER@MTA.AC.IL", MTA);
            var diklaB = new Teacher("דקלה ברק", 0, "", "DIKLABAR@MTA.AC.IL", MTA);
            var yairG = new Teacher("יאיר גוטליב", 0, "", "GOTTLIEB@MTA.AC.IL", MTA);
            var gilG = new Teacher("גיל גולדצויג", 0, "02-9902858", "GILIGOLD@MTA.AC.IL", MTA);
            var giladG = new Teacher("גלעד גל", 0, "", "GILADGAL@MTA.AC.IL", MTA);
            var avitalG = new Teacher("אביטל גרשפלד ליטוין", 0, "", "AVITALGR@MTA.AC.IL", MTA);
            var ravidD = new Teacher("רביד דורון", 0, "", "RAVIDDOR@MTA.AC.IL", MTA);
            var ilanaH = new Teacher("אילנה הירסטון", 0, "", "ILANAHAI@MTA.AC.IL", MTA);
            var yonatanH = new Teacher("יונתן הנדלזלץ", 0, "", "YONATANH@MTA.AC.IL", MTA);
            var micahelH = new Teacher("מיכאל הרלב", 0, "03-6409919", "MICHAELH@MTA.AC.IL", MTA);
            var naomiZiv = new Teacher("נעמי זיו", 0, "", "NAOMIZIV@MTA.AC.IL", MTA);
            var chenH = new Teacher("חן חגי", 0, "", "CHENHAGA@MTA.AC.IL", MTA);
            var samerH = new Teacher("סאמר חלבי", 0, "", "HALABISA@MTA.AC.IL", MTA);
            var samiH = new Teacher("סמי חמדאן", 0, "", "SAMIHAMD@MTA.AC.IL", MTA);
            var tzipiH = new Teacher("ציפי חנליס", 0, "", "TZIPIH@MTA.AC.IL", MTA);
            var iritH = new Teacher("עירית חרותי", 0, "", "IRITHERU@MTA.AC.IL", MTA);
            var sagitT = new Teacher("שגית טל", 0, "", "SAGE@MTA.AC.IL", MTA);
            var mosheTal = new Teacher("משה טלמון", 0, "", "TALMON@MTA.AC.IL", MTA);
            var mairavC = new Teacher("מירב כהן ציון", 0, "", "MAIRAVCO@MTA.AC.IL", MTA);
            var eyalC = new Teacher("איל כהן", 0, "", "EYAL@MTA.AC.IL", MTA);
            var elenK = new Teacher("אלן כץ", 0, "", "ELLENKA@MTA.AC.IL", MTA);
            var dalitLev = new Teacher("דלית לב ארי מרגלית", 0, "", "DALITLEV@MTA.AC.IL", MTA);
            var refaelL = new Teacher("רפאל לב", 0, "", "RAFFILE@MTA.AC.IL", MTA);
            var refaelS = new Teacher("רפאל שניר", 0, "", "RSNIR@MTA.AC.IL", MTA);
            var efratL = new Teacher("אפרת לביא", 0, "", "EFRATLAV@MTA.AC.IL", MTA);
            var avshalomK = new Teacher("אבשלום קורן", 0, "", "AVSHALOM@MTA.AC.IL", MTA);
            var dafnaP = new Teacher("דפנה פלטי", 0, "", "PALTIDAF@MTA.AC.IL", MTA);
            var niritKa= new Teacher("נירית צפורה קרה", 0, "", "NIRITKA@MTA.AC.IL", MTA);
            var hagiR = new Teacher("חגי רבינוביץ", 0, "", "HAGAIRA@MTA.AC.IL", MTA);
            var oferP = new Teacher("עופר פיין", 0, "03-6802551", "OFERF@MTA.AC.IL", MTA);
            var adiSagi = new Teacher("עדי שגיא", 0, "", "ADISA@MTA.AC.IL", MTA);
            var eduardoS = new Teacher("אדוארדו שילמן", 0, "", "EDUARDOS@MTA.AC.IL", MTA);
            var noaSha = new Teacher("נועה שחם", 0, "", "NOASHAHA@MTA.AC.IL", MTA);
            var miraLevis = new Teacher("מירה לויס", 0, "", "MIRALEVI@MTA.AC.IL", MTA);
            var rivkaR = new Teacher("רבקה רייכר עתיר", 0, "", "RERA@MTA.AC.IL", MTA);
            var yelenaSt = new Teacher("ילנה סטוקלין", 0, "", "ELENA@MTA.AC.IL", MTA);
            var galitLen = new Teacher("גלית לנדסהוט", 0, "", "GALITLAN@MTA.AC.IL", MTA);
            var aviadR = new Teacher("אביעד רוטבוים", 0, "", "AVIADRO@MTA.AC.IL", MTA);

            //no mail
            var amirP = new Teacher("אמיר פלק", 0, "", "", MTA);
            var yaelP = new Teacher("יעל פרץ", 0, "", "", MTA);
            var einatH = new Teacher("עינת חיים", 0, "", "", MTA);
            var sigalLevi = new Teacher("סיגל לוי", 0, "", "", MTA);
            var avigailLiv = new Teacher("אביגיל לבני עזר", 0, "", "", MTA);
            
            //מדעי הסיעוד
            var riadAbu = new Teacher("ריאד אבו רייקה", 0, "", "RIADAV@MTA.AC.IL", MTA);
            var iritAvishar = new Teacher("אירית אבישר", 0, "", "IRITAV@MTA.AC.IL", MTA);
            var talAlmog = new Teacher("טל אלמוג", 0, "", "TALAL@MTA.AC.IL", MTA);
            var aviadAp = new Teacher("אביעד אפלבאום", 0, "", "AVIADAP@MTA.AC.IL", MTA);
            var esterB = new Teacher("אסתר בהט", 0, "", "ESTHERBA@MTA.AC.IL", MTA);
            var veredBach = new Teacher("ורד בכר", 0, "", "VEREDBA@MTA.AC.IL", MTA);
            var aharonBenSh = new Teacher("אהרון בן שפרוט", 0, "", "RONIBENS@MTA.AC.IL", MTA);
            var osnatDavid = new Teacher("אוסנת דוד", 0, "", "OSNATDA@MTA.AC.IL", MTA);
            var ravidDoron = new Teacher("רביד דורון", 0, "", "RAVIDDOR@MTA.AC.IL", MTA);
            var yafeAh = new Teacher("יפה הארון", 0, "", "YAFAHA@MTA.AC.IL", MTA);
            var davidwi = new Teacher("דוד  וילהלם", 0, "", "DAVIDWI@MTA.AC.IL", MTA);
            var dalitWi= new Teacher("דלית וילהלם", 0, "", "DALITWL@MTA.AC.IL", MTA);
            var jalalT = new Teacher("ג'לאל טרביה", 0, "", "JALALTA@MTA.AC.IL", MTA);
            var benyaminLevi = new Teacher("בנימין לוי", 0, "", "BENLEVI@MTA.AC.IL", MTA);
            var ofirLazar = new Teacher("אופיר לזר", 0, "", "OFIRLAZA@MTA.AC.IL", MTA);
            var aviramMilin = new Teacher("אבירם מילין", 0, "", "AVIRAMMA@MTA.AC.IL", MTA);
            var yakirMis = new Teacher("יקיר מיסושצין", 0, "", "YAKIRMI@MTA.AC.IL", MTA);
            var gilaN = new Teacher("גילה ניקומרוב נחום", 0, "", "GILANA@MTA.AC.IL", MTA);
            var dimitriP = new Teacher("דימיטרי פיטרמן", 0, "", "DIMITRYFI@MTA.AC.IL", MTA);
            var mordechayFran = new Teacher("מרדכי פרנקו", 0, "", "MOTTYFRA@MTA.AC.IL", MTA);
            var erzsebetpr = new Teacher("ארז'בט פרצל", 0, "", "ERZSEBETPR@MTA.AC.IL", MTA);
            var daniKu = new Teacher("דני קוצוק", 0, "", "DANIKU@MTA.AC.IL", MTA);
            var kerenSha = new Teacher("קרן שחר", 0, "", "SHAKHARK@MTA.AC.IL", MTA);
            var borisSh = new Teacher("בוריס שניידמן", 0, "", "borchick100@gmail.com", MTA);
            var ataliaTu= new Teacher("עתליה תובל", 0, "", "ATALIATU@MTA.AC.IL", MTA);
            var yairGot = new Teacher("יאיר גוטליב", 0, "", "GOTTLIEB@MTA.AC.IL", MTA);
            var olegGolo= new Teacher("אולג גולולובוב", 0, "", "OLEGGO@MTA.AC.IL", MTA);
            var innaNe = new Teacher("אינה נשר", 0, "", "INNANE@MTA.AC.IL", MTA);
            var pazitAzuri= new Teacher("פזית עזורי", 0, "", "PAZITA@MTA.AC.IL", MTA);
            var ninaPitalik = new Teacher("נינה פיטליק", 0, "", "NINAPI@MTA.AC.IL", MTA);
            //no mail
            var taliVizer = new Teacher("טלי ויזר", 0, "", "-", MTA);
            var romanL = new Teacher("רומן לשינסקי", 0, "", "", MTA);
            var jenyN = new Teacher("ג'ני ניימרק", 0, "", "", MTA);
            var taliaB = new Teacher("טליה בקר", 0, "", "", MTA);
            var amitGutk = new Teacher("עמית גוטקינד", 0, "", "", MTA);
            var grover = new Teacher("גרובר שלמה מנחם שמחה", 0, "", "", MTA);
            var arada = new Teacher("חילמי עארדה", 0, "", "", MTA);
            var davidEzra = new Teacher("דוד עזרא", 0, "", "", MTA);
            

            //ממשל וחברה
            var efratA = new Teacher("אפרת עמירה", 0, "", "EFRATBER2@MTA.AC.IL", MTA);
            var noaL = new Teacher("נועה לביא", 0, "03-5230024", "LAVIE@MTA.AC.IL", MTA);
            var yaelA= new Teacher("יעל אברדם", 0, "", "YAELAE@MTA.AC.IL", MTA);
            var iritAdler = new Teacher("אירית אדלר", 0, "", "IRITADLE@MTA.AC.IL", MTA);
            var dganitOfek = new Teacher("דגנית אופק", 0, "", "DGANITOF@MTA.AC.IL", MTA);
            var matanOram = new Teacher("מתן אורם", 0, "", "ORAMM@MTA.AC.IL", MTA);
            var yuvalEit = new Teacher("יובל איתן", 0, "", "EYTANYU@MTA.AC.IL", MTA);
            var sagiElbaz = new Teacher("שגיא אלבז", 0, "", "SAGIEL@MTA.AC.IL", MTA);
            var akerman = new Teacher("יהושע אקרמן", 0, "", "AKERMANY@MTA.AC.IL", MTA);
            var shaulArieli = new Teacher("שאול אריאלי", 0, "", "SHAULAR@MTA.AC.IL", MTA);
            var doriBen = new Teacher("דורי בן זאב", 0, "", "DORIBENZ@MTA.AC.IL", MTA);
            var shlomitBn = new Teacher("שלומית בנימין", 0, "", "BENYAMIN@MTA.AC.IL", MTA);
            var tzviBarel = new Teacher("צבי בראל", 0, "", "ZVIBAREL@MTA.AC.IL", MTA);
            var tamarBar = new Teacher("תמר ברקאי", 0, "", "TAMARBAR@MTA.AC.IL", MTA);
            var lioraGvi = new Teacher("ליאורה גביעון", 0, "", "LIORAGVI@MTA.AC.IL", MTA);
            var danielGilon = new Teacher("דניאל גילון", 0, "", "DANIELGL@MTA.AC.IL", MTA);
            var lilianaGalili= new Teacher("ליליאנה גלילי", 0, "", "LILIGALI@MTA.AC.IL", MTA);
            var ariehGronik = new Teacher("אריה גרוניק", 0, "", "ARIEGERO@MTA.AC.IL", MTA);
            var zachGranit = new Teacher("צח גרניט", 0, "", "ZACHGRAN@MTA.AC.IL", MTA);
            var michaelDa = new Teacher("מיכאל דהאן", 0, "", "MIKEDAHA@MTA.AC.IL", MTA);
            var hanaHerzog = new Teacher("חנה הרצוג", 0, "03-6407383", "hherzog@mta.ac.il", MTA);
            var yaelHashilon = new Teacher("יעל השילוני דולב", 0, "", "YAELHASH@MTA.AC.IL", MTA);
            var noritYafe = new Teacher("נורית השמשוני יפה", 0, "", "NURITHAS@MTA.AC.IL", MTA);
            var grasiela = new Teacher("גרסיאלה טרכטנברג", 0, "", "GTRAJ@MTA.AC.IL", MTA);
            var hadadYaron = new Teacher("הדס ירון מסגנה", 0, "", "HADASYAR@MTA.AC.IL", MTA);
            var sharonHagbi = new Teacher("שרון חגבי", 0, "", "SHARONHA@MTA.AC.IL", MTA);
            var yochebedYosef = new Teacher("יוכבד יוסף", 0, "", "YOSEFYOC@MTA.AC.IL", MTA);
            var daniYong = new Teacher("דני יונג", 0, "", "DANNYYOU@MTA.AC.IL", MTA);
            var chenHagi = new Teacher("חן חגי", 0, "", "CHENHAGA@MTA.AC.IL", MTA);
            var tamarTau = new Teacher("תמר טאובר פאוזנר", 0, "", "TAUBERPA@MTA.AC.IL", MTA);
            var oferCassif = new Teacher("עופר כסיף", 0, "", "OFERCASS@MTA.AC.IL", MTA);
            var tamarArev = new Teacher("תמר ערב", 0, "", "TAMARARE@MTA.AC.IL", MTA);
            var netanelFisher = new Teacher("נתנאל פישר", 0, "", "NETANELFI@MTA.AC.IL", MTA);
            var mayaPin = new Teacher("מאיה פנחסי", 0, "", "MAYAPI@MTA.AC.IL", MTA);
            var korenAvshalom = new Teacher("קורן אבשלום", 0, "", "AVSHALOM@MTA.AC.IL", MTA);
            var ariehKram= new Teacher("אריה קרמפף", 0, "", "ARIEKR@MTA.AC.IL", MTA);
            var rachelRomberg = new Teacher("רחל רומברג", 0, "", "RAQUELRO@MTA.AC.IL", MTA);
            var tamarShifter = new Teacher("תמר שיפטר סגיב", 0, "", "TAMARSHI@MTA.AC.IL", MTA);
            var omriShamir = new Teacher("עומרי שמיר", 0, "", "OMRISH@MTA.AC.IL", MTA);
            var sheliS = new Teacher("שלי שנהב", 0, "", "SHELLYSH@MTA.AC.IL", MTA);
            var erezShak = new Teacher("ארז שקלר", 0, "", "EREZSHKL@MTA.AC.IL", MTA);
            var yelenaStoklin = new Teacher("ילנה סטוקלין", 0, "", "ELENA@MTA.AC.IL", MTA);
            var noamSeger = new Teacher("נעם סגר", 0, "", "NOAMSEGE@MTA.AC.IL", MTA);
            var rinaNeman = new Teacher("רינה נאמן", 0, "", "RINANE@MTA.AC.IL", MTA);
            var mosheUziel = new Teacher("משה עוזיאל", 0, "", "uzielmos@mail.mta.ac.il", MTA);
            var assafMidani = new Teacher("אסף מידני", 0, "", "ASSAFMEI@MTA.AC.IL", MTA);
            var tamarLar = new Teacher("תמר לרנטל", 0, "03-6407262", "TAMARLAR@MTA.AC.IL", MTA);
            var dianaLu = new Teacher("דיאנה לוצטו", 0, "", "LUZZATTO@MTA.AC.IL", MTA);
            var rutiYuval = new Teacher("רותי יובל", 0, "", "RUTHYU@MTA.AC.IL", MTA);
            var neomiHazan = new Teacher("נעמי חזן", 0, "", "NCHAZAN@MTA.AC.IL", MTA);
            var yusriK= new Teacher("יוסרי ח'יזראן", 0, "", "YUSRIKH@MTA.AC.IL", MTA);
            var zivAnat= new Teacher("זיו ענת", 0, "", "ANATZI@MTA.AC.IL", MTA);

            //no mail
            var heziB = new Teacher("חזי ביצור", 0, "", "", MTA);
            var batelE = new Teacher("בתאל אשקול", 0, "", "", MTA);
            var oranaDon = new Teacher("אורנה דונת", 0, "", "", MTA);
            var shaiHa = new Teacher("שי הבט", 0, "", "", MTA);
            var amitKaplan = new Teacher("עמית קפלן", 0, "", "", MTA);
            var omereKinan = new Teacher("עומר קינן", 0, "", "", MTA);
            var yoramRon= new Teacher("יורם רון", 0, "", "", MTA);
            var beniA = new Teacher("בני עמנו אוראל", 0, "", "", MTA);
            var tamirMagal = new Teacher("תמיר מגל", 0, "", "", MTA);
            var sagiLatin = new Teacher("שגיא לטין", 0, "", "", MTA);
            var aditL= new Teacher("עדית לבנה", 0, "", "", MTA);
            var malkaLevi = new Teacher("מלכה לוי טל", 0, "", "", MTA);
            var yaelaLa = new Teacher("יעלה להב רז", 0, "", "", MTA);
            var haimL = new Teacher("חיים לבנברג", 0, "", "", MTA);
            
            
            //ניהול מערכות מידע
            var sergeyA = new Teacher("סרגיי אליינוב", 0, "", "SERGEYAE@MTA.AC.IL", MTA);
            var talA = new Teacher("טל אספיר", 0, "", "TALA@MTA.AC.IL", MTA);
            var danielAriel = new Teacher("דניאל אריאל", 0, "", "danielar@mta.ac.il", MTA);
            var meravEsh= new Teacher("מירב אש", 0, "", "MERAVHAS@MTA.AC.IL", MTA);
            var dafniBiran = new Teacher("דפני בירן אחיטוב", 0, "", "BIRANAHITUVDA@MTA.AC.IL", MTA);
            var erezBenMoshe = new Teacher("ארז בן משה", 0, "", "EREZBENM@MTA.AC.IL", MTA);
            var itzikBar = new Teacher("איציק בר נוי", 0, "", "ITZIKBAR@MTA.AC.IL", MTA);
            var anatGold = new Teacher("ענת גולדשטיין", 0, "", "ANATGO@MTA.AC.IL", MTA);
            var michaekGor = new Teacher("מיכאל גורקוב", 0, "", "MICHAELG@MTA.AC.IL", MTA);
            var hananGit = new Teacher("חנן גיטליץ", 0, "", "hanangi@mta.ac.il", MTA);
            var rutiGafni = new Teacher("רותי גפני", 0, "", "RUTIGAFN@MTA.AC.IL", MTA);
            var naomiDadon = new Teacher("נעמי דאדון", 0, "", "NAOMIDAD@MTA.AC.IL", MTA);
            var davidDnieli = new Teacher("דוד דניאלי", 0, "", "DAVIDDAN@MTA.AC.IL", MTA);
            var danWin = new Teacher("דן וינר", 0, "", "danwi@mta.ac.il", MTA);
            var adiZamir = new Teacher("עדי זמיר", 0, "", "ADIZM@MTA.AC.IL", MTA);
            var zipiS = new Teacher("ציפי שפרלינג", 0, "", "ZIPIS@MTA.AC.IL", MTA);
            var heliS = new Teacher("חלי שלסקי", 0, "", "chelishe@mail.mta.ac.il", MTA);
            var jimmy = new Teacher("חיים ג'ימי שוורצקופ", 0, "", "JIMMYSCH@MTA.AC.IL", MTA);
            var adiR = new Teacher("עדי ריינהולד", 0, "", "ADIRINHO@MTA.AC.IL", MTA);
            var yoavR = new Teacher("יואב רשף", 0, "", "YOAVRESH@MTA.AC.IL", MTA);
            var yonitR = new Teacher("יונית רושו", 0, "", "YONITRU@MTA.AC.IL", MTA);
            var nuritH = new Teacher("נורית חלוזין", 0, "", "NURITCHA@MTA.AC.IL", MTA);
            var arturYosef = new Teacher("ארתור יוסף", 0, "", "ARTHURYO@MTA.AC.IL", MTA);
            var kerenK = new Teacher("קרן כליף", 0, "", "KERENK@MTA.AC.IL", MTA);
            var sagiL= new Teacher("שגיא לוי", 0, "", "Sagi5182182@gmail.com", MTA);
            var dorMeir = new Teacher("דור מאיר", 0, "", "DORMEIR@MTA.AC.IL", MTA);
            var shaunM= new Teacher("שון מלאך", 0, "", "shaunmal@mta.ac.il", MTA);
            var madiN = new Teacher("מדי נבון", 0, "", "MADINA@MTA.AC.IL", MTA);
            var noaN = new Teacher("נועה נלסון", 0, "", "noane@mta.ac.il", MTA);
            var noamS = new Teacher("נעם סגר", 0, "", "NOAMSEGE@MTA.AC.IL", MTA);
            var marinaFani = new Teacher("מרינה פאני", 0, "", "MARINAFA@MTA.AC.IL", MTA);

            //nomail
            var ayaAlu= new Teacher("איה אלוביץ", 0, "", "", MTA);
            var batyaBen = new Teacher("בתיה בן הדור ויינברג", 0, "", "", MTA);
            var galitGord = new Teacher("גלית גורדוני", 0, "", "", MTA);
            var avishaiH = new Teacher("אבישי חג'בי", 0, "", "", MTA);
            var ronRozen = new Teacher("רון רוזן", 0, "", "", MTA);
            var bernardK = new Teacher("ברנרד קאופמן דוד", 0, "", "", MTA);
            var danaL = new Teacher("דנה לנדאו", 0, "", "", MTA);
            var luizaM = new Teacher("לואיזה מלייב", 0, "", "", MTA);
            var orM = new Teacher("אור מנדלסון", 0, "", "", MTA);
            var oriMarmi = new Teacher("אורי מרמי", 0, "", "", MTA);
            var orenN = new Teacher("אורן נחום", 0, "", "", MTA);
            var benN = new Teacher("בן נובו שלם", 0, "", "", MTA);
            var talP = new Teacher("טל פבל", 0, "", "", MTA);


            //כלכלה וניהול
            var efratLa = new Teacher("אפרת לאלו", 0, "", "EFRATLAL@MTA.AC.IL", MTA);
            var einatLavi = new Teacher("עינת לביא", 0, "", "", MTA);
            var galitAvidar = new Teacher("גלית אבידר", 0, "", "GALITAVI@MTA.AC.IL", MTA);
            var niritA = new Teacher("נירת אגאי", 0, "", "NIRITAGA@MTA.AC.IL", MTA);
            var galAh = new Teacher("גל אהרן", 0, "", "GALAHARO@MTA.AC.IL", MTA);
            var liadOr = new Teacher("ליעד אורתר", 0, "", "LIADOR@MTA.AC.IL", MTA);
            var yaacovEr = new Teacher("יעקב ארז", 0, "", "YAACOVER@MTA.AC.IL", MTA);
            var baruchE = new Teacher("ברוך אשל", 0, "", "BARUCHES@MTA.AC.IL", MTA);
            var israelB = new Teacher("ישראל בורוביץ", 0, "", "ISRAELB@MTA.AC.IL", MTA);
            var ranB = new Teacher("רן ביצ'צ'י", 0, "", "RANBICHA@MTA.AC.IL", MTA);
            var gadiBir = new Teacher("גדי בירקנפלד", 0, "", "gadibi@mta.ac.il", MTA);
            var yosefL = new Teacher("יוסף לוי", 0, "", "YOSILEVI@MTA.AC.IL", MTA);
            var zafrir = new Teacher("צפריר אפרים בלוך דוד", 0, "", "zafrirbl@mta.ac.il", MTA);
            var eyalB = new Teacher("אייל בנימין", 0, "", "BENJAMIN@MTA.AC.IL", MTA);
         /*   var = new Teacher("", 0, "", "", MTA);
            var = new Teacher("", 0, "", "", MTA);
            var = new Teacher("", 0, "", "", MTA);
            var = new Teacher("", 0, "", "", MTA);
            var  = new Teacher("", 0, "", "", MTA);*/

            var teachers = new[]
            {
                //מדעי המחשב - 57
                romina,oren, ofer, shlomit, baruch, hadar,liron, amirBen, michal, aharon,carmit, yossi, nili, omer, michalB
                ,iris, toma, uri, osnat,gidon, roi,rotem, ishai,uziel, arieh,wissam,lubov,shmuel, hilel, guy, boaz,
                ariehLev, dani, borisL, davidM, tzvi,carmi,alonN,hilaN, zef, raid, alex, adiA, avishaiF, doritP, alexComan,
                ilanK,amirK, barakK, gilK,roditi, guyRonen, leonidR, adiShabat, ester, heli, adiShrivman,

                //מדעי ההתנהגות-53
                avidar, ronitA,sabrina, michalOr,aviA,odeliaE, HaimA, sofiaA,shaiB,hedvaB,dafnaB, diklaB, yairG, gilG, giladG,
                avitalG, ravidD,ilanaH, yonatanH,micahelH, naomiZiv,chenH,samerH, samiH,tzipiH,iritH, sagitT, mosheTal,
                mairavC, eyalC, elenK, dalitLev, refaelL, refaelS,efratL, avshalomK,dafnaP,niritKa,hagiR,oferP,adiSagi, miraLevis,
                eduardoS, noaSha, rivkaR, yelenaSt, galitLen,aviadR,amirP,yaelP,einatH,sigalLevi,avigailLiv,
               
                //סיעוד-38
                riadAbu,iritAvishar,talAlmog,aviadAp,esterB,veredBach,aharonBenSh,osnatDavid,ravidDoron,yafeAh,taliVizer,davidwi,
                dalitWi,jalalT,benyaminLevi,ofirLazar,romanL,aviramMilin,jenyN,yakirMis,gilaN,taliaB,dimitriP,mordechayFran,
                erzsebetpr,daniKu,kerenSha,borisSh,ataliaTu,yairGot,amitGutk,grover,olegGolo,innaNe,arada,pazitAzuri,davidEzra,
                ninaPitalik,

                // ממשל וחברה -67
                efratA, noaL,yaelA,iritAdler,dganitOfek,matanOram,yuvalEit,sagiElbaz,akerman,shaulArieli,doriBen,shlomitBn,
                tzviBarel,tamarBar,lioraGvi,danielGilon,lilianaGalili,ariehGronik,zachGranit,michaelDa,hanaHerzog,hanaHerzog,
                yaelHashilon, noritYafe,grasiela,hadadYaron,sharonHagbi,yochebedYosef,daniYong,chenHagi,tamarTau,oferCassif,
                tamarArev, netanelFisher, mayaPin,korenAvshalom,ariehKram,rachelRomberg,tamarShifter,omriShamir,sheliS,
                erezShak, yelenaStoklin,noamSeger,rinaNeman,mosheUziel,assafMidani,tamarLar,dianaLu,rutiYuval,neomiHazan,
                yusriK,zivAnat,heziB, batelE, oranaDon, shaiHa, amitKaplan, omereKinan, yoramRon, beniA, tamirMagal,
                sagiLatin, aditL,malkaLevi,yaelaLa, haimL,
            
                //ניהול מערכות מידע -44
                ayaAlu,sergeyA,talA,danielAriel,meravEsh,dafniBiran,erezBenMoshe,batyaBen,itzikBar,anatGold,michaekGor,hananGit,
                rutiGafni,naomiDadon,davidDnieli,danWin,adiZamir,galitGord,avishaiH,zipiS,heliS,jimmy,adiR,yoavR,yonitR,nuritH,
                arturYosef,kerenK,sagiL,dorMeir,shaunM,madiN,noaN,noamS,marinaFani,ronRozen,bernardK,danaL
                ,luizaM,orM,oriMarmi,orenN,benN,talP,

                //כלכלה וניהול-14
                efratLa,einatLavi,galitAvidar,niritA,galAh,yaacovEr,liadOr,baruchE,israelB,ranB,gadiBir,yosefL,zafrir,eyalB,

               /* new Teacher {Name = "אמיר קירש"},
                new Teacher {Name = "יוסי בצלאל"},
                new Teacher {Name = "צבי מלמד"},
                new Teacher {Name = "כרמי"},
                new Teacher {Name ="הדר בינסקי"},
                new Teacher {Name ="בוריס לוין"},
                new Teacher {Name ="אלכס קומן"},*/
            };

            foreach (var teacher in teachers)
            {
                session.Save(teacher);
            }

            

            var logic = new Course
            {
                University = MTA,
                Name = "לוגיקה",
                AcademicDegree = AcademicDegree.Bachelor,
                Faculty = computersFaculty,
                IntendedYear = IntendedYear.First,
                IsMandatory = true,
            };

            var courses = new[]
            {
                logic,
                new Course
                {
                    University = MTA,
                    Name = "אלגוריתמים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Second,
                    IsMandatory = true
                },
                new Course
                {
                    University = MTA,
                    Name = "תורת הגרפים",
                    AcademicDegree = AcademicDegree.Bachelor,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.Any,
                    IsMandatory = false
                },
                new Course
                {
                    University = MTA,
                    Name = "סיבוכיות ",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = computersFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                 new Course
                {
                    University = MTA,
                    Name = "ביולוגיה",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = socialityFaculty,
                    IntendedYear = IntendedYear.First,
                    IsMandatory = true
                },
                new Course
                {
                    University = MTA,
                    Name = "מדעים",
                    AcademicDegree = AcademicDegree.Master,
                    Faculty = socialityFaculty,
                    IntendedYear = IntendedYear.Third,
                    IsMandatory = true
                },
            };

            var courseComment = new CourseComment
            {
                CommentText = "קורס ממש מעניין",
                DateTime = DateTime.Now,
                Votes = { new Vote(true) }
            };

            foreach (var courseCriteria in CourseComment.GetCourseCommentCriterias())
            {
                courseComment.CriteriaRatings.Add(new CourseCriteriaRating(courseCriteria, 5));
            }

            logic.CourseInSemesters.Add(new CourseInSemester
            {
                Semester = Semester.A,
                Teacher = romina,
                Course = logic,
                Year = 2012,
            });

            logic.AddCourseCommnet(logic.CourseInSemesters[0], courseComment);

            foreach (var course in courses)
            {
                session.Save(course);
            }
        }
    }
}