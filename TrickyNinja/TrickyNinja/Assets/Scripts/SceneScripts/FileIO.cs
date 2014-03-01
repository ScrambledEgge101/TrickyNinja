using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public static class FileIO 
{
	static string profilesPath;

	public static ProfileContainer profileContainer;

	public static void AddToContainer( Profile aProfile )
	{
		if( profileContainer == null)
		{
			profileContainer = new ProfileContainer();
		}
		profileContainer.profiles.Add( aProfile );
	}

	public static void LoadProfiles()
	{
		if( profileContainer == null)
		{
			profileContainer = new ProfileContainer();
		}
		profileContainer = profileContainer.Load( GetProfilesPath() );
	}

	public static void SaveProfiles()
	{
		if( profileContainer == null)
		{
			profileContainer = new ProfileContainer();
		}
		profileContainer.Save( GetProfilesPath() );
	}

	public static string GetProfilesPath()
	{
		if( profilesPath == null || profilesPath == "" )
		{
			Debug.Log("Profilespath not set");
			return "Profiles.xml";
		}
		return profilesPath;
	}

	public static void SetProfilesPath(string aPath)
	{
		profilesPath = aPath;
	}

	public static Profile GetProfileByName( string aName)
	{
		foreach( Profile p in profileContainer.profiles )
		{
			if( p.name == aName )
			{
				return p;
			}
		}
		return new Profile();
	}
}

public class ProfileContainer
{
	[XmlArray("Profiles")]
	[XmlArrayItem("Profile")]

	public List<Profile> profiles = new List<Profile>();

	public ProfileContainer Load( string path )
	{
		var serializer = new XmlSerializer( typeof( ProfileContainer ) );
		var stream = new FileStream( path , FileMode.Open );
		var container = serializer.Deserialize(stream) as ProfileContainer;
		stream.Close();
		return container;
	}

	public void Save( string path )
	{
		var serializer = new XmlSerializer( typeof( ProfileContainer ) );
		var stream = new FileStream( path , FileMode.Create ) ;
		serializer.Serialize( stream, this);
		stream.Close();
	}
}