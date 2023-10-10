using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UI.Dates;

public class PatientList : MonoBehaviour
{
    [Header("Patient List")]
    [SerializeField]
    private GameObject patientPrefab;
    [SerializeField]
    private Transform scrollContent;
    [SerializeField]
    private TMP_Text downloadStatus;
    [SerializeField]
    private Button retry;

    [Header("Patient Info")]
    [SerializeField]
    private GameObject patientInfo;
    [SerializeField]
    private TMP_Text patientName;
    [SerializeField]
    private TMP_Text patientBirthdate;
    [SerializeField]
    private TMP_Text patientGender;
    [SerializeField]
    private TMP_Text patientHandedness;

    [Header("New Patient")]
    [SerializeField]
    private DatePicker birthdatePicker;
    [SerializeField]
    private Button newPatientBtn;

    private PatientData[] patients = new PatientData[0];

    // Start is called before the first frame update
    // ♀ female
    // ♂ male
    private void Start()
    {
        StartCoroutine(DownloadAllPatients());

        newPatientBtn.onClick.AddListener(delegate
        {
            birthdatePicker.SelectedDate = new SerializableDate();
            StartCoroutine(DownloadAllPatients());
        });
    }

    public void ManualDownloadAllPatients()
    {
        StartCoroutine(DownloadAllPatients());
        retry.gameObject.SetActive(false);
        downloadStatus.SetText("Loading...");
    }

    private IEnumerator DownloadAllPatients()
    {
        PatientData patientData = new PatientData();
        yield return StartCoroutine(patientData.DownloadAllPatients(result =>
        {
            if (!string.IsNullOrEmpty(result))
            {
                patients = JsonHelper.FromJson<PatientData>(result);
            }
            else
            {
                downloadStatus.SetText("Cannot download patient list.");
                retry.gameObject.SetActive(true);
            }

            downloadStatus.gameObject.SetActive(false);
        }));

        foreach (Transform child in scrollContent)
            Destroy(child.gameObject);

        foreach (PatientData pData in patients)
            GeneratePatient(pData);
    }

    private void OpenPatientInfo(string fullName, string birthdate, string gender, string handedness)
    {
        patientName.text = fullName;
        patientBirthdate.text = birthdate;
        patientGender.text = gender;
        patientHandedness.text = handedness;

        patientInfo.SetActive(true);
    }

    public void TestPatient()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void GeneratePatient(PatientData pData)
    {
        PatientButton patient = Instantiate(patientPrefab, Vector3.zero, Quaternion.identity, scrollContent).GetComponent<PatientButton>();
        Button patientButton = patient.GetComponent<Button>();

        System.DateTime birthdate = System.DateTime.Now;
        try
        {
            birthdate = System.DateTime.Parse(pData.birthDate);
        }
        catch
        {
            Debug.Log("Cannot format " + pData.firstName + " " + pData.lastName + "'s birthdate\nDefaulting to date today");
        }

        string formattedBirthdate = birthdate.ToString("MMMM dd, yyyy");

        patientButton.onClick.AddListener(delegate
        {
            Patient.instance.SetPatientData(pData);
            OpenPatientInfo(pData.GetFullName(), formattedBirthdate, pData.gender, pData.handedness);
        });

        patient.SetPatientData(pData.gender.Equals("Male") ? "♂" : "♀", pData.GetFullName(), formattedBirthdate);
    }

    public void DeletePatient()
    {
        Popup popup = Popup.instance;
        Patient patient = Patient.instance;
        popup.OpenPopup("Are you sure you want to delete " + patient.GetName() + "'s records?", PopupType.YESNO);
        popup.YesButton().AddListener(delegate { StartCoroutine(ConfirmDelete()); });
    }

    private IEnumerator ConfirmDelete()
    {
        PatientData patientData = Patient.instance.GetPatientData();
        yield return StartCoroutine(patientData.Delete(result =>
        {
            if (result)
            {
                ManualDownloadAllPatients();
            }
            else
            {
                Debug.Log("Cannot delete");
            }
        }));
    }

    public void UpdateList(string query)
    {
        foreach (Transform child in scrollContent)
        {
            child.gameObject.SetActive(false);

            PatientButton patient = child.gameObject.GetComponent<PatientButton>();
            if (patient != null && patient.GetName().ToUpper().Contains(query.ToUpper()))
                patient.gameObject.SetActive(true);
        }
    }
}
