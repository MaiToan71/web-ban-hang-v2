import type { CSSProperties } from "react";

type Props = {
  cover: string;
  subtitle: string;
  title: string;
  style?: CSSProperties;
};

export default function AnalysisCard({ cover, subtitle, title, style }: Props) {
  return (
    <div
      className="flex items-center rounded-2xl py-10 p-3"
      style={{
        ...style,
      }}
    >
      <div className="flex flex-col">
        <span className="text-xl font-bold">{title}</span>
        <span className="text-sm">{subtitle}</span>
      </div>
      <img src={cover} alt="" />
    </div>
  );
}
