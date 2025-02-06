using System.Collections.Generic;
using UnityEngine;

public class ItemSorteio : MonoBehaviour
{
    private int somaPesos;

    private int numeroAleatorio;

    private int somaAcumulada;
    public List<Item> itens = new();

    [System.Serializable]
    public class Item
    {
        public string nome;
        public int peso;

        public Item(string nome, int peso)
        {
            this.nome = nome;
            this.peso = peso;
        }
    }

    void Start()
    {
        AddNewItem("10", 1);
        AddNewItem("2", 100);
        AddNewItem("1", 200);
        AddNewItem("0", 500);
    }

    public void AddNewItem(string nome, int peso)
    {
        itens.Add(new Item(nome, peso));
    }

    public Item Sortearitem()
    {
        somaPesos = 0;
        foreach (Item item in itens)
        {
            somaPesos += item.peso;
        }

        numeroAleatorio = Random.Range(0, somaPesos);

        somaAcumulada = 0;

        foreach (Item item in itens)
        {
            somaAcumulada += item.peso;
            if (numeroAleatorio < somaAcumulada)
            {
                return item;
            }
        }

        return null;
    }

    public Item Simulation()
    {
        somaPesos = 0;
        foreach (Item item in itens)
        {
            somaPesos += item.peso;
        }

        numeroAleatorio = Random.Range(0, somaPesos);

        somaAcumulada = 0;

        foreach (Item item in itens)
        {
            somaAcumulada += item.peso;
            if (numeroAleatorio < somaAcumulada)
            {
                return item;
            }
        }

        return null;
    }

    public void SortearItemSimulation(int value)
    {
        Item item;
        for (int i = 0; i < value; i++)
        {
            item = Simulation();
            Debug.Log(item.nome);
        }
    }
}
