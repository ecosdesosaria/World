using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public class ScrollClue : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Scroll; } }

		public string ScrollText;
		public string ScrollSolved;
		public int ScrollIntelligence;
		public int ScrollLevel;
		public int ScrollTrue;
		public string ScrollDescribe;
		public string ScrollQuest;
		public string ScrollCharacter;
		public int ScrollX;
		public int ScrollY;
		public Map ScrollMap;

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Text { get { return ScrollText; } set { ScrollText = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Solved { get { return ScrollSolved; } set { ScrollSolved = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_Intelligence { get { return ScrollIntelligence; } set { ScrollIntelligence = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_Level { get { return ScrollLevel; } set { ScrollLevel = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_True { get { return ScrollTrue; } set { ScrollTrue = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Describe { get { return ScrollDescribe; } set { ScrollDescribe = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Quest { get { return ScrollQuest; } set { ScrollQuest = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Character { get { return ScrollCharacter; } set { ScrollCharacter = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_X { get { return ScrollX; } set { ScrollX = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_Y { get { return ScrollY; } set { ScrollY = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Map Scroll_Map { get{ return ScrollMap; } set{ ScrollMap = value; } }

		[Constructable]
		public ScrollClue( ) : base( 0x4CC6 )
		{
			Weight = 1.0;
			Name = "a parchment";
			ItemID = Utility.RandomList( 0x4CC6, 0x4CC7 );

			if ( ScrollLevel > 0 ){} else
			{
				string sLanguage = "pixie";
				switch( Utility.RandomMinMax( 0, 28 ) )
				{
					case 0: sLanguage = "balron"; break;
					case 1: sLanguage = "pixie"; break;
					case 2: sLanguage = "centaur"; break;
					case 3: sLanguage = "demonic"; break;
					case 4: sLanguage = "dragon"; break;
					case 5: sLanguage = "dwarvish"; break;
					case 6: sLanguage = "elven"; break;
					case 7: sLanguage = "fey"; break;
					case 8: sLanguage = "gargoyle"; break;
					case 9: sLanguage = "cyclops"; break;
					case 10: sLanguage = "gnoll"; break;
					case 11: sLanguage = "goblin"; break;
					case 12: sLanguage = "gremlin"; break;
					case 13: sLanguage = "druidic"; break;
					case 14: sLanguage = "tritun"; break;
					case 15: sLanguage = "minotaur"; break;
					case 16: sLanguage = "naga"; break;
					case 17: sLanguage = "ogrish"; break;
					case 18: sLanguage = "orkish"; break;
					case 19: sLanguage = "sphinx"; break;
					case 20: sLanguage = "treekin"; break;
					case 21: sLanguage = "trollish"; break;
					case 22: sLanguage = "undead"; break;
					case 23: sLanguage = "vampire"; break;
					case 24: sLanguage = "dark elf"; break;
					case 25: sLanguage = "magic"; break;
					case 26: sLanguage = "human"; break;
					case 27: sLanguage = "symbolic"; break;
					case 28: sLanguage = "runic"; break;
				}

				string sPart = "a ";
				switch( Utility.RandomMinMax( 0, 6 ) )
				{
					case 0:	sPart = "a strange ";	break;
					case 1:	sPart = "an odd ";		break;
					case 2:	sPart = "an ancient ";	break;
					case 3:	sPart = "a long dead ";	break;
					case 4:	sPart = "a cryptic ";	break;
					case 5:	sPart = "a mystical ";	break;
					case 6:	sPart = "a symbolic ";	break;
				}

				string sCalled = "a strange";
				switch( Utility.RandomMinMax( 0, 6 ) )
				{
					case 0: sCalled = "an odd"; break;
					case 1: sCalled = "an unusual"; break;
					case 2: sCalled = "a bizarre"; break;
					case 3: sCalled = "a curious"; break;
					case 4: sCalled = "a peculiar"; break;
					case 5: sCalled = "a strange"; break;
					case 6: sCalled = "a weird"; break;
				}

				Name = sCalled + " parchment";

				ScrollDescribe = "written in " + sPart + sLanguage + " language";
				ScrollIntelligence = Utility.RandomMinMax( 2, 8 ) * 10;
				ScrollLevel = ( ScrollIntelligence / 10 ) - 1;

				if ( ScrollIntelligence >= 80 ){ ScrollSolved = "Diabolically Coded"; }
				else if ( ScrollIntelligence >= 70 ){ ScrollSolved = "Ingeniously Coded"; }
				else if ( ScrollIntelligence >= 60 ){ ScrollSolved = "Deviously Coded"; }
				else if ( ScrollIntelligence >= 50 ){ ScrollSolved = "Cleverly Coded"; }
				else if ( ScrollIntelligence >= 40 ){ ScrollSolved = "Adeptly Coded"; }
				else if ( ScrollIntelligence >= 30 ){ ScrollSolved = "Expertly Coded"; }
				else { ScrollSolved = "Plainly Coded"; }

				int scrollWords = Utility.RandomMinMax( 1, 4 );
				ScrollTrue = 0; if ( Utility.RandomMinMax( 1, 2 ) == 1 ){ ScrollTrue = 1; }

				if ( scrollWords == 1 )
				{
					Land land = Land.Sosaria;
					switch ( Utility.Random( 9 ) )
					{
						case 0: land = Land.Sosaria; break;
						case 1: land = Land.Lodoria; break;
						case 2: land = Land.Serpent; break;
						case 3: land = Land.IslesDread; break;
						case 4: land = Land.Savaged; break;
						case 5: land = Land.Ambrosia; break;
						case 6: land = Land.UmberVeil; break;
						case 7: land = Land.Kuldar; break;
						case 8: land = Land.Underworld; break;
					}
					Point3D loc = Worlds.GetRandomLocation( land, "land" );

					ScrollX = loc.X;
					ScrollY = loc.Y;
					ScrollMap = Worlds.GetMyDefaultTreasureMap( land );
					ScrollQuest = "grave";

					Point3D spot = new Point3D(ScrollX, ScrollY, 0);
					int xLong = 0, yLat = 0;
					int xMins = 0, yMins = 0;
					bool xEast = false, ySouth = false;

					string my_location = "";

					if ( Sextant.Format( spot, ScrollMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
					{
						my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
					}

					switch ( Utility.Random( 3 ) )
					{
						case 0: ScrollCharacter = QuestCharacters.ParchmentWriter(); ScrollText = "Dear " + QuestCharacters.ParchmentWriter() + ",<br><br>It saddens me to write this message to you and I hope it finds you well. " + ScrollCharacter + " was killed while we were exploring " + QuestCharacters.SomePlace( "parchment" ) + " and I have done everything I could for them. I could not bring them with me as the journey to " + RandomThings.GetRandomCity() + " was too far and I did not want the animals to pick the bones clean, so I buried them in a shallow grave in " + Server.Lands.LandName( land ) + ". If you wish to visit the grave, the location is at...<br><br>" + my_location + "<br><br>Make sure to bring a grave shovel if you wish to return them to your home in " + RandomThings.GetRandomCity() + " and read this parchment when you get there to make sure you are in the right spot. Again, I am sorry for your loss.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						case 1: ScrollCharacter = QuestCharacters.ParchmentWriter(); ScrollText = QuestCharacters.ParchmentWriter() + ",<br><br>This message is for you eyes only, I would appreciate discretion in this matter. " + ScrollCharacter + " tried to kill me while we were traveling " + Server.Lands.LandName( land ) + ", but I bested them in battle. Before returning to " + RandomThings.GetRandomCity() + ", I buried them to keep others from asking questions of their whereabouts. I also buried them with their belongings so if they had something you needed, you can find their body at...<br><br>" + my_location + "<br><br>Make sure to bring a grave shovel, and bury it back up when you are done. Make sure to read this parchment when you get there to make sure you are in the right spot. I already have enough troubles.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						case 2: ScrollCharacter = QuestCharacters.ParchmentWriter(); ScrollText = QuestCharacters.ParchmentWriter() + ",<br><br>The plan worked! I led " + ScrollCharacter + " out into " + Server.Lands.LandName( land ) + " and killed them before they even knew what I was doing. I buried them and went back to " + RandomThings.GetRandomCity() + " to collect my payment, but they said you needed proof of the deed. Very well. I buried the body at...<br><br>" + my_location + "<br><br>So you can go see for yourself. Make sure to bring a grave shovel and read this parchment when you get there to make sure you are in the right spot. Then meet me in " + RandomThings.GetRandomCity() + " where I will be waiting for my gold. Don't take too long, or I may have to do a job for myself.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					}
				}
				else if ( scrollWords == 2 )
				{
					Land land = Land.Sosaria;
					switch ( Utility.Random( 9 ) )
					{
						case 0: land = Land.Sosaria; break;
						case 1: land = Land.Lodoria; break;
						case 2: land = Land.Serpent; break;
						case 3: land = Land.IslesDread; break;
						case 4: land = Land.Savaged; break;
						case 5: land = Land.Ambrosia; break;
						case 6: land = Land.UmberVeil; break;
						case 7: land = Land.Kuldar; break;
						case 8: land = Land.Underworld; break;
					}
					Point3D loc = Worlds.GetRandomLocation( land, "land" );

					ScrollX = loc.X;
					ScrollY = loc.Y;
					ScrollMap = Worlds.GetMyDefaultTreasureMap( land );
					ScrollQuest = "chest";

					Point3D spot = new Point3D(ScrollX, ScrollY, 0);
					int xLong = 0, yLat = 0;
					int xMins = 0, yMins = 0;
					bool xEast = false, ySouth = false;

					string my_location = "";

					if ( Sextant.Format( spot, ScrollMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
					{
						my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
					}

					string sVillain = "thieves";
					string sHero = "a ranger";
					string sBody = "foot";
					string sMonster = "an orc";
					switch ( Utility.RandomMinMax( 0, 4 ) )
					{
						case 0: sVillain = "thieves"; 	sHero = "a ranger";		sBody = "foot";		sMonster = "an orc";		break;
						case 1: sVillain = "rogues";	sHero = "a guard"; 		sBody = "leg";		sMonster = "an ogre";		break;
						case 2: sVillain = "robbers";	sHero = "a knight"; 	sBody = "hand";		sMonster = "a troll";		break;
						case 3: sVillain = "brigands";	sHero = "a peasant"; 	sBody = "head";		sMonster = "a lizardman";	break;
						case 4: sVillain = "bandits";	sHero = "a mercenary"; 	sBody = "arm";		sMonster = "an ettin";		break;
					}

					switch ( Utility.Random( 3 ) )
					{
						case 0: ScrollCharacter = QuestCharacters.QuestGiver(); ScrollText = "Dear " + QuestCharacters.ParchmentWriter() + ",<br><br>I made it to " + RandomThings.GetRandomCity() + " last night after being chased by some " + sVillain + " through " + Server.Lands.LandName( land ) + ". In order to not lose everything I was traveling with, I had to bury it. I think they knew what I had in that chest because I see them around asking questions about me. It is up to you to get to it before they do. I will attempt to lead them away while you head for...<br><br>" + my_location + "<br><br>Make sure to bring a shovel and read this parchment when you get there to make sure you are in the right spot. Once I lose them I will return home. Keep that one thing safe above all else.<br><br> - " + ScrollCharacter + ""; break;
						case 1: ScrollCharacter = QuestCharacters.ParchmentWriter(); ScrollText = QuestCharacters.ParchmentWriter() + ",<br><br>I managed to follow " + ScrollCharacter + " all the way to " + RandomThings.GetRandomCity() + " where they stayed for " + Utility.RandomMinMax( 2, 8 ) + " days. When they finally departed, I followed them through " + Server.Lands.LandName( land ) + " and waited for the right moment. Although things did not go well for them, I managed to get that chest you wanted. What I didn't know was " + sHero + " saw me and gave chase. I managed to lose them long enough to bury the goods and make it back to " + RandomThings.GetRandomCity() + " without getting caught. Although they are still searching for me, they know not who you are. If you can dig it up at...<br><br>" + my_location + "<br><br>I will gladly accept half the gold as payment since I could not deliver it to you myself. Make sure to bring a shovel and read this parchment when you get there to make sure you are in the right spot.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						case 2: ScrollCharacter = QuestCharacters.QuestGiver(); ScrollText = QuestCharacters.ParchmentWriter() + ",<br><br>I followed all of the clues and it led me to " + RandomThings.GetRandomCity() + " where I found the final piece of the map. It shows that " + ScrollCharacter + " buried their treasure in " + Server.Lands.LandName( land ) + " and it may still be there. I am resting in " + RandomThings.GetRandomCity() + " as I write this because I severely hurt my " + sBody + " in " + sMonster + " attack yesterday. As soon as you can, go to...<br><br>" + my_location + "<br><br>Make sure to bring a shovel and read this parchment when you get there to make sure you are in the right spot. If that item is in the chest as the legends say, then bring it to " + QuestCharacters.SomePlace( "parchment" ) + " and I will meet you there.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					}
				}
				else if ( scrollWords == 3 )
				{
					Land land = Land.Sosaria;
					switch ( Utility.Random( 9 ) )
					{
						case 0: land = Land.Sosaria; break;
						case 1: land = Land.Lodoria; break;
						case 2: land = Land.Serpent; break;
						case 3: land = Land.IslesDread; break;
						case 4: land = Land.Savaged; break;
						case 5: land = Land.Ambrosia; break;
						case 6: land = Land.UmberVeil; break;
						case 7: land = Land.Kuldar; break;
						case 8: land = Land.Underworld; break;
					}

					Point3D loc = Worlds.GetRandomLocation( land, "sea" );

					Map shipMap = Worlds.GetMyDefaultTreasureMap( land );

					switch ( Utility.Random( 31 ) )
					{
						case 0: loc = new Point3D(578, 1370, -5); shipMap = Map.Underworld; break;
						case 1: loc = new Point3D(946, 821, -5); shipMap = Map.SavagedEmpire; break;
						case 2: loc = new Point3D(969, 217, -5); shipMap = Map.SavagedEmpire; break;
						case 3: loc = new Point3D(322, 661, -5); shipMap = Map.SavagedEmpire; break;
						case 4: loc = new Point3D(760, 587, -5); shipMap = Map.IslesDread; break;
						case 5: loc = new Point3D(200, 1056, -5); shipMap = Map.IslesDread; break;
						case 6: loc = new Point3D(1232, 387, -5); shipMap = Map.IslesDread; break;
						case 7: loc = new Point3D(528, 233, -5); shipMap = Map.IslesDread; break;
						case 8: loc = new Point3D(504, 1931, -5); shipMap = Map.SerpentIsland; break;
						case 9: loc = new Point3D(1472, 1776, -5); shipMap = Map.SerpentIsland; break;
						case 10: loc = new Point3D(1560, 579, -5); shipMap = Map.SerpentIsland; break;
						case 11: loc = new Point3D(1328, 144, -5); shipMap = Map.SerpentIsland; break;
						case 12: loc = new Point3D(2312, 2299, -5); shipMap = Map.Lodor; break;
						case 13: loc = new Point3D(2497, 3217, -5); shipMap = Map.Lodor; break;
						case 14: loc = new Point3D(576, 3523, -5); shipMap = Map.Lodor; break;
						case 15: loc = new Point3D(4352, 3768, -5); shipMap = Map.Lodor; break;
						case 16: loc = new Point3D(4824, 1627, -5); shipMap = Map.Lodor; break;
						case 17: loc = new Point3D(3208, 216, -5); shipMap = Map.Lodor; break;
						case 18: loc = new Point3D(1112, 619, -5); shipMap = Map.Lodor; break;
						case 19: loc = new Point3D(521, 2153, -5); shipMap = Map.Lodor; break;
						case 20: loc = new Point3D(2920, 1643, -5); shipMap = Map.Lodor; break;
						case 21: loc = new Point3D(320, 2288, -5); shipMap = Map.Sosaria; break;
						case 22: loc = new Point3D(3343, 1842, -5); shipMap = Map.Sosaria; break;
						case 23: loc = new Point3D(3214, 938, -5); shipMap = Map.Sosaria; break;
						case 24: loc = new Point3D(4520, 1128, -5); shipMap = Map.Sosaria; break;
						case 25: loc = new Point3D(4760, 2307, -5); shipMap = Map.Sosaria; break;
						case 26: loc = new Point3D(3551, 2952, -5); shipMap = Map.Sosaria; break;
						case 27: loc = new Point3D(1271, 2651, -5); shipMap = Map.Sosaria; break;
						case 28: loc = new Point3D(744, 1304, -5); shipMap = Map.Sosaria; break;
						case 29: loc = new Point3D(735, 555, -5); shipMap = Map.Sosaria; break;
						case 30: loc = new Point3D(1824, 440, -5); shipMap = Map.Sosaria; break;
					}

					int shipX = loc.X;
					int shipY = loc.Y;
					Point3D spot = new Point3D(shipX, shipY, 0);
					int xLong = 0, yLat = 0;
					int xMins = 0, yMins = 0;
					bool xEast = false, ySouth = false;

					string my_location = "";

					if ( Sextant.Format( spot, shipMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
					{
						my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
					}

					switch ( Utility.Random( 3 ) )
					{
						case 0: ScrollText = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Estávamos navegando em alto mar em " + Server.Lands.LandName( land ) + ", quando notamos que nossa âncora ficou presa em algo. A água estava clara então podíamos ver quase até o fundo do mar. Estávamos presos em um navio que encontrou seu fim. Passamos cerca de " + Utility.RandomMinMax( 2, 8 ) + " dias, pescando o que podíamos dos destroços.<br><br>" + my_location + "<br><br>Encontramos alguns itens interessantes. Um de nossos tripulantes, que era membro da guilda dos marinheiros, parecia trazer os itens mais valiosos. Estávamos ficando sem comida então rumamos para a costa. Com minha parte do espólio, agora estou aqui em " + RandomThings.GetRandomCity() + ", escrevendo esta tão esperada carta para você. Pare aqui quando puder, pois tenho uma proposta para você.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						case 1: ScrollText = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Encontramos nosso antigo navio enquanto estávamos em alto mar em " + Server.Lands.LandName( land ) + ". O sol estava favorável, e podíamos ver o mastro próximo à superfície da esteira. Passamos cerca de " + Utility.RandomMinMax( 2, 8 ) + " dias tentando recuperar o que podíamos.<br><br>" + my_location + "<br><br>Recuperamos muitas de nossas coisas. Um de nossos tripulantes, que era membro da guilda dos marinheiros, parecia trazer o saque do capitão. A água estava ficando agitada, então rumamos para o cais mais próximo. Se puder me encontrar em " + RandomThings.GetRandomCity() + ", posso devolver algumas de suas coisas que você perdeu naquele dia fatídico.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						case 2: ScrollText = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>As lendas eram verdadeiras! Aquele " + RandomThings.GetRandomJob() + " maluco sabia onde estava aquele navio e eu o encontrei. Tive que levar um pequeno barco até lá, mas pude distinguir sua sombra sob as ondas.<br><br>" + my_location + "<br><br>Não posso esperar recuperar nada dos destroços, mas talvez consiga com a ajuda do seu pai. Venha até " + RandomThings.GetRandomCity() + ", e elaboraremos um plano para comprar um navio e voltar pelo tesouro.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					}
				}
				else
				{
					int xSet = 1;
					int ySet = 1;
					Map mSet = Map.Sosaria;
					string vMap = "";
					string vHome = "a dungeon";

					int HomeLocation = Utility.RandomMinMax( 1, 4 );

					if ( HomeLocation == 1 )
					{
						ScrollTrue = 1;
						vHome = "a castle in the sky";
						switch ( Utility.Random( 7 ) )
						{
							case 0: xSet = 1863; ySet = 1129; mSet = Map.Sosaria; break;
							case 1: xSet = 1861; ySet = 2747; mSet = Map.Lodor; break;
							case 2: xSet = 466; ySet = 3801; mSet = Map.Lodor; break;
							case 3: xSet = 254; ySet = 670; mSet = Map.SerpentIsland; break;
							case 4: xSet = 422; ySet = 398; mSet = Map.SavagedEmpire; break;
							case 5: xSet = 251; ySet = 1249; mSet = Map.IslesDread; break;
							case 6: xSet = 3884; ySet = 2879; mSet = Map.Sosaria; break;
						}
					}
					else if ( HomeLocation == 2 )
					{
						ScrollTrue = 1;
						vHome = "a dungeon";
						switch ( Utility.Random( 21 ) )
						{
							case 0: xSet = 4299; ySet = 3318; mSet = Map.Lodor; break;
							case 1: xSet = 177; ySet = 961; mSet = Map.SavagedEmpire; break;
							case 2: xSet = 766; ySet = 1527; mSet = Map.SavagedEmpire; break;
							case 3: xSet = 1191; ySet = 1516; mSet = Map.SerpentIsland; break;
							case 4: xSet = 1944; ySet = 3377; mSet = Map.Sosaria; break;
							case 5: xSet = 1544; ySet = 1785; mSet = Map.SerpentIsland; break;
							case 6: xSet = 2059; ySet = 2406; mSet = Map.Sosaria; break;
							case 7: xSet = 1558; ySet = 2861; mSet = Map.Lodor; break;
							case 8: xSet = 755; ySet = 1093; mSet = Map.IslesDread; break;
							case 9: xSet = 2181; ySet = 1327; mSet = Map.Sosaria; break;
							case 10: xSet = 752; ySet = 680; mSet = Map.SavagedEmpire; break;
							case 11: xSet = 466; ySet = 3801; mSet = Map.Lodor; break;
							case 12: xSet = 2893; ySet = 2030; mSet = Map.Lodor; break;
							case 13: xSet = 1050; ySet = 93; mSet = Map.SavagedEmpire; break;
							case 14: xSet = 127; ySet = 85; mSet = Map.IslesDread; break;
							case 15: xSet = 145; ySet = 1434; mSet = Map.SerpentIsland; break;
							case 16: xSet = 2625; ySet = 823; mSet = Map.Lodor; break;
							case 17: xSet = 740; ySet = 182; mSet = Map.IslesDread; break;
							case 18: xSet = 5390; ySet = 3280; mSet = Map.Sosaria; break;
							case 19: xSet = 922; ySet = 1775; vMap = "the Hedge Maze"; mSet = Map.SavagedEmpire; break;
							case 20: xSet = 1036; ySet = 1162; mSet = Map.SavagedEmpire; break;
						}
					}
					else // FALSE LOCATION
					{
						ScrollTrue = 0;
						switch ( Utility.Random( 2 ) )
						{
							case 0: vHome = "a dungeon"; break;
							case 1: vHome = "a castle in the sky"; break;
						}

						Land land = Land.Sosaria;
						switch ( Utility.Random( 7 ) )
						{
							case 0: land = Land.Sosaria; break;
							case 1: land = Land.Lodoria; break;
							case 2: land = Land.Serpent; break;
							case 3: land = Land.IslesDread; break;
							case 4: land = Land.Savaged; break;
							case 5: land = Land.Ambrosia; break;
							case 6: land = Land.UmberVeil; break;
						}
						Point3D loc = Worlds.GetRandomLocation( land, "land" );
						xSet = loc.X; ySet = loc.Y; mSet = Worlds.GetMyDefaultTreasureMap( land );
					}

					Point3D location = new Point3D(xSet, ySet, 0);
					if ( vMap == "" ){ vMap = Server.Lands.LandName( Server.Lands.GetLand( mSet, location, xSet, ySet ) ); }
					int xLong = 0, yLat = 0;
					int xMins = 0, yMins = 0;
					bool xEast = false, ySouth = false;

					Point3D spot = new Point3D(xSet, ySet, 0);
					string my_location = location.ToString();

					if ( Sextant.Format( spot, mSet, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
					{
						my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
					}

					string sBuilding = "torre";
					switch( Utility.RandomMinMax( 0, 6 ) )
					{
						case 0: sBuilding = "torre"; break;
						case 1: sBuilding = "casa"; break;
						case 2: sBuilding = "fortaleza"; break;
						case 3: sBuilding = "castelo"; break;
						case 4: sBuilding = "cabana"; break;
						case 5: sBuilding = "mansão"; break;
						case 6: sBuilding = "tenda"; break;
					}

					ScrollText = QuestCharacters.ParchmentWriter() + ",<br><br>Durante minha última jornada, encontrei algo bastante notável. Deparei-me com " + vHome + " que parecia estar abandonado(a). Acredito que era " + vHome + " que uma vez pertenceu a " + QuestCharacters.QuestGiver() + ". Nossa " + sBuilding + " próxima a " + RandomThings.GetRandomCity() + " está ficando muito pequena para as pilhas de ouro que temos conseguido ao explorar " + QuestCharacters.SomePlace( "parchment" ) + ", então provavelmente é hora de nos mudarmos. Acho que já temos quase ouro suficiente para contratar a ajuda necessária para deixar o lugar pronto para morar, então gostaria que você me encontrasse em " + RandomThings.GetRandomCity() + " onde podemos procurar trabalhadores para nos ajudar neste empreendimento. Se puder fazer a viagem, deveria ver por si mesmo. Se ainda tiver aquele sextante, pode encontrá-lo em " + vMap + " em...<br><br>" + my_location + "<br><br> - " + QuestCharacters.QuestGiver() + "";
				}
			}

			if ( ScrollX == 0 || ScrollY == 0 || ScrollMap == null )
				Delete();
		}

		static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1070722, ScrollDescribe);
			list.Add( 1049644, ScrollSolved); // PARENTHESIS
		}

		public class ClueGump : Gump
		{
			private Item m_Item;
			private Map m_Map;
			private int m_X;
			private int m_Y;

			public ClueGump( Mobile from, Item parchment ): base( 100, 100 )
			{
				from.CloseGump( typeof( ClueGump ) );
				ScrollClue scroll = (ScrollClue)parchment;

				m_Item = parchment;
				m_Map = scroll.ScrollMap;
				m_X = scroll.ScrollX;
				m_Y = scroll.ScrollY;

				string sText = scroll.ScrollText;
				from.PlaySound( 0x249 );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 10901, 2786);
				AddImage(0, 0, 10899, 2117);
				AddHtml( 45, 78, 386, 218, @"<BODY><BASEFONT Color=#d9c781>" + sText + "</BASEFONT></BODY>", (bool)false, (bool)true);

				if ( Sextants.HasSextant( from ) )
					AddButton(377, 325, 10461, 10461, 1, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 

				if ( info.ButtonID > 0 )
				{
					from.CloseGump( typeof( Sextants.MapGump ) );
					from.SendGump( new Sextants.MapGump( from, m_Map, m_X, m_Y, null ) );
					from.SendGump( new ClueGump( from, m_Item ) );
				}
				else
				{
					from.PlaySound( 0x249 );
					from.CloseGump( typeof( Sextants.MapGump ) );
				}
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage( "This must be in your backpack to read." );
				return;
			}
			else
			{
				if ( ( e.Int >= ScrollIntelligence && ScrollIntelligence > 0 ) || e.Skills[SkillName.Inscribe].Value >= Utility.RandomMinMax( 30, 120 ) )
				{
					if ( ScrollIntelligence >= 80 ){ Name = "diabolically coded parchment"; }
					else if ( ScrollIntelligence >= 70 ){ Name = "ingeniously coded parchment"; }
					else if ( ScrollIntelligence >= 60 ){ Name = "deviously coded parchment"; }
					else if ( ScrollIntelligence >= 50 ){ Name = "cleverly coded parchment"; }
					else if ( ScrollIntelligence >= 40 ){ Name = "adeptly coded parchment"; }
					else if ( ScrollIntelligence >= 30 ){ Name = "expertly coded parchment"; }
					else { Name = "plainly coded parchment"; }

					ScrollIntelligence = 0;
					ScrollSolved = "Deciphered by " + e.Name;
					InvalidateProperties();
				}

				if ( ScrollIntelligence < 1 )
				{
					// GRAVE SEARCH QUEST
					if ( ScrollQuest == "grave" && e.X >=(ScrollX-2) && e.X <=(ScrollX+2) && e.Y >=(ScrollY-2) && e.Y <=(ScrollY+2) && e.Map == ScrollMap )
					{
						Item graveshovel = e.FindItemOnLayer( Layer.TwoHanded );

						if ( graveshovel == null )
						{
							e.SendMessage("Você precisa estar segurando uma pá de cova!");
						}
						else if ( graveshovel != null && !(graveshovel is GraveSpade) )
						{
							e.SendMessage("Você precisa estar segurando uma pá de cova!");
						}
						else if ( e.Mounted )
						{
							e.SendMessage("Você não consegue cavar muito bem quando está em uma montaria!");
						}
						else if ( e.IsBodyMod && !e.Body.IsHuman && e.RaceID < 1 )
						{
							e.SendMessage("Você não consegue cavar muito bem enquanto está transformado.");
						}
						else if ( ScrollTrue == 0 )
						{
							e.PlaySound( 0x125 );
							e.Animate( 14, 5, 1, true, false, 0 );
							switch( Utility.RandomMinMax( 0, 6 ) )
							{
								case 0: e.SendMessage("Parece que alguém já esteve aqui, então você joga fora o pergaminho."); break;
								case 1: e.SendMessage("A mensagem deve ter sido uma mentira, então você joga fora o pergaminho."); break;
								case 2: e.SendMessage("Isso deve ter sido uma farsa, então você joga fora o pergaminho."); break;
								case 3: e.SendMessage("Obviamente não há nada aqui, então você joga fora o pergaminho."); break;
								case 4: e.SendMessage("Você nota um buraco vazio por perto, então você joga fora o pergaminho."); break;
								case 5: e.SendMessage("Isso parece ter sido uma perda de tempo, então você joga fora o pergaminho."); break;
								case 6: e.SendMessage("Você viajou todo esse caminho por nada, então você joga fora o pergaminho."); break;
							}
							this.Delete();
						}
						else
						{
							e.SendMessage("Você encontrou o corpo logo abaixo do chão, então não tem mais uso para o pergaminho.");
							Item chest = new BuriedBody( ScrollLevel*2, ScrollCharacter, e );
							LoggingFunctions.LogQuestBody( e, ScrollCharacter );
							chest.MoveToWorld( e.Location, e.Map );
							e.PlaySound( 0x125 );
							e.Animate( 14, 5, 1, true, false, 0 );
							this.Delete();
						}
					}

					// CHEST SEARCH QUEST
					if ( ScrollQuest == "chest" && e.X >=(ScrollX-2) && e.X <=(ScrollX+2) && e.Y >=(ScrollY-2) && e.Y <=(ScrollY+2) && e.Map == ScrollMap )
					{
						Item shovel = e.FindItemOnLayer( Layer.TwoHanded );

						if ( shovel == null )
						{
							e.SendMessage("Você precisa estar segurando uma pá!");
						}
						else if ( shovel != null && !(shovel is Spade) )
						{
							e.SendMessage("Você precisa estar segurando uma pá!");
						}
						else if ( e.Mounted )
						{
							e.SendMessage("Você não consegue cavar muito bem quando está em uma montaria!");
						}
						else if ( e.IsBodyMod && !e.Body.IsHuman && e.RaceID < 1 )
						{
							e.SendMessage("Você não pode cavar muito bem enquanto estiver transformado.");
						}
						else if ( ScrollTrue == 0 )
						{
							e.PlaySound( 0x125 );
							e.Animate( 14, 5, 1, true, false, 0 );
							switch( Utility.RandomMinMax( 0, 6 ) )
							{
								case 0: e.SendMessage("Parece que alguém já esteve aqui, então você joga fora o pergaminho."); break;
								case 1: e.SendMessage("A mensagem deve ter sido uma mentira, então você joga fora o pergaminho."); break;
								case 2: e.SendMessage("Isso deve ter sido uma farsa, então você joga fora o pergaminho."); break;
								case 3: e.SendMessage("Obviamente não há nada aqui, então você joga fora o pergaminho."); break;
								case 4: e.SendMessage("Você nota um buraco vazio por perto, então você joga fora o pergaminho."); break;
								case 5: e.SendMessage("Isso parece ter sido uma perda de tempo, então você joga fora o pergaminho."); break;
								case 6: e.SendMessage("Você viajou todo esse caminho por nada, então você joga fora o pergaminho."); break;
							}
							this.Delete();
						}
						else
						{
							e.SendMessage("Você encontrou o baú logo abaixo do chão, então não tem mais uso para o pergaminho.");
							Item chest = new BuriedChest( ScrollLevel*2, ScrollCharacter, e );
							chest.MoveToWorld( e.Location, e.Map );
							LoggingFunctions.LogQuestChest( e, ScrollCharacter );
							e.PlaySound( 0x125 );
							e.Animate( 14, 5, 1, true, false, 0 );
							this.Delete();
						}
					}

					else { e.SendGump( new ClueGump( e, this ) ); e.PlaySound( 0x249 ); }
				}
				else { e.SendMessage("Você não parece entender o que está escrito aqui!"); }
			}
		}

		public ScrollClue(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
            writer.Write( ScrollText );
			writer.Write( ScrollSolved );
            writer.Write( ScrollIntelligence );
            writer.Write( ScrollLevel );
            writer.Write( ScrollTrue );
            writer.Write( ScrollDescribe );
            writer.Write( ScrollQuest );
            writer.Write( ScrollCharacter );
            writer.Write( ScrollX );
            writer.Write( ScrollY );
			writer.Write( (Map) ScrollMap );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
            ScrollText = reader.ReadString();
			ScrollSolved = reader.ReadString();
            ScrollIntelligence = reader.ReadInt();
            ScrollLevel = reader.ReadInt();
			ScrollTrue = reader.ReadInt();
            ScrollDescribe = reader.ReadString();
			ScrollQuest = reader.ReadString();
			ScrollCharacter = reader.ReadString();
			ScrollX = reader.ReadInt();
			ScrollY = reader.ReadInt();
			ScrollMap = reader.ReadMap();

			if ( ItemID != 0x4CC6 && ItemID != 0x4CC7 ){ ItemID = Utility.RandomList( 0x4CC6, 0x4CC7 ); }
			Hue = 0;

			if ( ScrollX == 0 || ScrollY == 0 || ScrollMap == null )
				Delete();
		}
	}
}