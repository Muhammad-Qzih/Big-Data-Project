
// tereger the api to fetch data

function getDataTop() {
    fetch('http://localhost:5082/api/Tweets/top')
        .then(response => response.json())
        .then(dataList => {

            document.getElementById('result1').innerHTML = '';
            createPieChart(dataList);
        }) 
        .catch(error => {
            console.error('Error fetching data:', error);
        });
}

// generate pie chart using charts.js and view the data by this chart
function createPieChart(dataList) {
    const existingCanvas = document.getElementById('pieChart');
    if (existingCanvas) {
        existingCanvas.remove();
    }

    const canvas = document.createElement('canvas');
    canvas.id = 'pieChart1';
    document.getElementById('result1').appendChild(canvas);

    const ctx = canvas.getContext('2d');

    const data = {
        labels: dataList.map(data => data.userName),
        datasets: [{
            data: dataList.map(data => data.tweetCount),
            backgroundColor: [
                'rgba(255, 99, 132, 0.8)',
                'rgba(54, 162, 235, 0.8)',
                'rgba(255, 206, 86, 0.8)',
                'rgba(75, 192, 192, 0.8)',
                'rgba(153, 102, 255, 0.8)',
                'rgba(255, 159, 64, 0.8)',
                'rgba(0, 123, 255, 0.8)',
                'rgba(255, 69, 0, 0.8)',
                'rgba(34, 139, 34, 0.8)',
                'rgba(255, 0, 255, 0.8)',
                'rgba(128, 0, 0, 0.8)',
                'rgba(0, 128, 0, 0.8)',
                'rgba(0, 0, 128, 0.8)',
                'rgba(128, 128, 0, 0.8)',
                'rgba(128, 0, 128, 0.8)',
                'rgba(0, 128, 128, 0.8)',
                'rgba(192, 192, 192, 0.8)',
                'rgba(128, 128, 128, 0.8)',
                'rgba(255, 165, 0, 0.8)',
                'rgba(0, 255, 255, 0.8)',
            ],
            
        }],
    };

    
    const options = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                labels: {
                    fontSize: 12, 
                    color: 'black', 
                },
            },
        },
    };


    new Chart(ctx, {
        type: 'pie',
        data: data,
        options: options,
    });
}
