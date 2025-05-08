using System.Reflection.Emit;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class SpawnEditor : Editor
{
    void OnSceneGUI(){
        EnemyManager manager = (EnemyManager)target;

        if(manager.spawnLists[manager.listID] == null) return;

        for(int i = 0; i < manager.spawnLists[manager.listID].list.Count; i++){
            var data = manager.spawnLists[manager.listID].list[i];

            Handles.color = Color.black;
            Handles.Label(data.position + Vector3.up, string.Format("ID : {0}",data.id));

            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(data.position,Quaternion.identity);

            if(EditorGUI.EndChangeCheck()){
                Undo.RecordObject(manager.spawnLists[manager.listID],"Move Spawn Point");
                manager.spawnLists[manager.listID].list[i].position = newPos;
            }
        }
    }
}
