﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinBind: MonoBehaviour
{
	public bool isBinding;
	public KinMol boundMol;
	public Transform bindingSite;
	public int typeToBind;
	public float affinity;
	public bool isReleaseSite;

	public KinMol testMolecule;
	public KinMol lastBindableMolecule;

	// Start is called before the first frame update
	void Start()
	{
		isBinding = false;
	}


	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.GetComponent("KinMol") as KinMol)
		{
			testMolecule = collider.gameObject.GetComponent("KinMol") as KinMol;
			DoBindCheck(testMolecule);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent("KinMol") as KinMol == lastBindableMolecule)
		{
			lastBindableMolecule = null;
		}
	}


	private void DoBindCheck(KinMol molecule)
	{
		//if (!isBinding)
		{
			//KinMol molecule = collider.gameObject.GetComponent("KinMol") as KinMol;
			if (molecule)
			{
				if (molecule.type == typeToBind)
				{
					//bool displace = Random.Range(0f, 1f) > 0.9f;
					if (!isBinding)// || displace)
					{
						// if I am already binding then release my bound molecule
						if (isBinding)
						{
							boundMol.myKinBind.ReleaseMol();
						}

						// if molecule is already bound to another site - force release
						if (molecule.myKinBind)
						{
							molecule.myKinBind.ReleaseMol();
						}

						BindMol(molecule);
						//var averagePosition = (collider.gameObject.transform.position + gameObject.transform.position) / 2f;
						//mySpawner.SpawnNewMolecule(3, averagePosition);

						//Destroy(gameObject);
						//Destroy(collider.gameObject);
					}
					else
					{
						lastBindableMolecule = testMolecule;
					}


				}
			}
		}
	}

	private void BindMol(KinMol molecule)
	{
		isBinding = true;
		boundMol = molecule;
		molecule.myKinBind = this;
	}

	public void ReleaseMol()
	{
		if (boundMol)
		{
			KinMol _lastBoundMolecule = boundMol;

			isBinding = false;
			boundMol.myKinBind = null;
			boundMol = null;

			if (lastBindableMolecule && (lastBindableMolecule != _lastBoundMolecule))
			{
				DoBindCheck(lastBindableMolecule);
			}
			
		}
		else
		{
			Debug.LogError("ReleaseMol() - boundMol NULL");
		}
	}

	void CheckForExit()
	{
		if (isReleaseSite && isBinding)
		{
			if (bindingSite.GetComponent<Collider>().bounds.Intersects(boundMol.GetComponent<Collider>().bounds))
			{
				ReleaseMol();
			}

		}
	}

	void CheckForMissedTrigger()
	{
		if (!isBinding && lastBindableMolecule)
		{
			Debug.Log("MISSED TRIGGER");
			//DoBindCheck(lastBindableMolecule);
		}
	}


	// Update is called once per frame
	void Update()
	{
		CheckForExit();
		CheckForMissedTrigger();
	}
}
