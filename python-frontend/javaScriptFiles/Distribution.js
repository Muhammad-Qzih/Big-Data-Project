// tereger the api to fetch data

function getDataDistribution() {
    const userNameFilter = document.getElementById('filterInput').value;

    fetch(`http://localhost:5082/api/Tweets/distribution?userName=${userNameFilter}`)
        .then(response => response.json())
        .then(dataList => {
            document.getElementById('result2').innerHTML = '';
             console.log(dataList)   
            updateTrendChart(dataList);
        })
        .catch(error => {
            console.error('Error fetching data:', error);
        });
}

// generate trend chart using charts.js and view the data by this chart
function updateTrendChart(data) {
    const existingCanvas = document.getElementById('lineChart1');
    if (existingCanvas) {
        existingCanvas.remove();
    }
    const canvas = document.createElement('canvas');
    canvas.id = 'lineChart1';

    document.getElementById('result2').appendChild(canvas);
    const ctx = canvas.getContext('2d');
    const labels = data.map(item => {
        const date = new Date(item.distributionId.date);
        const formattedDate = date.toISOString().split('T')[0];
        return formattedDate;
    });
    const counts = data.map(item => item.countTweets);
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Tweets Over Time',
                data: counts,
                borderColor:    "rgba(255, 0, 0, 1)",
                borderWidth: 3,
                fill: false
            }]
        },
        options: options,

    });
}

const options =  {
    scales: {
        x: {
            type: 'category', 
            title: {
                display: true,
                text: 'Date',
                font: {
                    weight: 'bold', 
                    size: 20 
                }
            },
        },
        y: {
            beginAtZero: true,
            title: {
                display: true,
                text: 'Number of Tweets',
                font: {
                    weight: 'bold', 
                    size: 20 
                }
            },
        },
    },
}