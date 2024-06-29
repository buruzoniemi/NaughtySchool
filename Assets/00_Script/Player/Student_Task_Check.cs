using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student_Task_Check : MonoBehaviour
{
    // �ϐ��錾------------------------------
    [SerializeField] private TextAsset textAsset; //�ǂލ��ރe�L�X�g���������܂�Ă���.txt�t�@�C��		
					 
    private string loadText; //Resources�t�H���_���璼�ڃe�L�X�g��ǂݍ���					 			
    private string[] splitText; //���s�ŕ������Ĕz��ɓ����						 
    private int textNum; //���ݕ\�����e�L�X�g�ԍ�
    private List<string> studentTaskList = new List<string>(); // task�Ǘ���List�ɕύX �C���X�y�N�^�[���task�𑝂₷���Ƃ��\
    private Dictionary<string, bool> studentTask = new Dictionary<string, bool>(); // List�̒��g��bool�Ǘ��ł���悤����ϐ�iD
    //--------------------------------------

    /// <summary>
    /// �ŏ��ɏ������郁�\�b�h
    /// </summary>
    void Start()
    {
        //������
        loadText = (Resources.Load("TaskRistText", typeof(TextAsset)) as TextAsset).text;
        splitText = loadText.Split(char.Parse("\n"));
        textNum = 0;

        //����������������
        ResetTasks();
        //���X�g������������
        ResetList();
    }

    /// <summary>
    /// ���t���[���������郁�\�b�h
    /// </summary>
    void Update()
    {
        //Debug_Manager.instance.DebugLog($"{studentTask.Keys}");
    }

    /// <summary>
    /// ���X�g�̒���Text�t�@�C���̂��̂�������
    /// </summary>
    private void ResetList()
    {
        studentTaskList.Clear(); // List���N���A���ď�����
            if (splitText[textNum] != "")
            {
                studentTaskList.AddRange(splitText);
                textNum++;
            }
    }

    /// <summary>
    /// ���������������鏈��
    /// </summary>
    private void ResetTasks()
    {
        studentTask.Clear(); // �������N���A���ď�����

        // studentTaskList���̊e�^�X�N�������ɒǉ����A������Ԃł͖�����(false)�ɐݒ肷��
        foreach (string task in studentTaskList)
        {
            studentTask.Add(task, false);
        }
    }

    /// <summary>
    ///  ����̃^�X�N�������ς݂ɂ���֐�
    /// </summary>
    /// <param name="taskName">List�̃^�X�N�̒��g</param>
    public void MarkTaskCompleted(string taskName)
    {
        if (studentTask.ContainsKey(taskName))
        {
            studentTask[taskName] = true; // �w�肳�ꂽ�^�X�N�������ς�(true)�ɐݒ肷��
        }
        else
        {
            Debug_Manager.instance.DebugLog("���̃^�X�N��������܂���: " + taskName);
        }
    }

    /// <summary>
    /// ����̃^�X�N�������ς݂��ǂ������擾����֐�
    /// </summary>
    /// <param name="taskName">List�̃^�X�N�̒��g</param>
    /// <returns></returns>
    public bool IsTaskCheck(string taskName)
    {
        if (studentTask.ContainsKey(taskName))
        {
            return studentTask[taskName]; // �w�肳�ꂽ�^�X�N�̊�����Ԃ�Ԃ�
        }
        else
        {
            Debug_Manager.instance.DebugLog("���̃^�X�N��������܂���: " + taskName);
            return false; // �^�X�N��������Ȃ��ꍇ�A������(false)��Ԃ�
        }
    }
}
