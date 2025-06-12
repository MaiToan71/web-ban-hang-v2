import Chart from "@/components/chart/chart";
import useChart from "@/components/chart/useChart";

export default function ChartLine({ data }: any) {
  const series = [
    {
      name: "Tiền cọc",
      data: data.map((i: any) => {
        return i.Value;
      }),
    },
  ];
  const chartOptions = useChart({
    xaxis: {
      categories: data.map((i: any) => {
        return `${i.Day}-${i.Month}-${i.Year}`;
      }),
    },
    tooltip: {
      x: {
        show: false,
      },
      marker: { show: false },
    },
  });

  return (
    <Chart type="line" series={series} options={chartOptions} height={350} />
  );
}
